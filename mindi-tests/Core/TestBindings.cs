using System;
using System.Collections.Generic;
using System.Threading;
using MinDI.Binders;
using NUnit.Framework;
using UnityEngine;

namespace MinDI.Tests
{
    [TestFixture]
    [Category("Unit tests")]
    internal class TestBindings {

		private enum MyEnum {
			Han,
			Viktoria,
			Zane
		}

		public class TestBinder : BaseDIBinder
		{
			public TestBinder (IDIContext context) : base (context)
			{
				this.instantiationType = Introspection.InstantiationType.Abstract;
				this.customFactoryWrapper = (t, f) => {
					return new AnotherClass (f ());
				};
			}
		}

		private interface IMyClass {}

		private class MyClass : IMyClass {
		}

		private class AnotherClass : IMyClass
		{
			public object data { get; set; }

			public AnotherClass (object data)
			{
				this.data = data;
			}
		}

		[Test]
		public void TestValueInstanceBinding() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().BindInstance<int>(5);

			int i = context.Resolve<int>();
			Assert.AreEqual(5, i);

			context.s().BindInstance<MyEnum>(MyEnum.Viktoria);
			MyEnum name = context.Resolve<MyEnum>();
			Assert.AreEqual(MyEnum.Viktoria, name);
		}

		[Test]
		public void TestInstanceBindingST() {
			MyClass obj = new MyClass();

			IDIContext context = ContextHelper.CreateContext();
			context.s().BindInstance<IMyClass>(obj);

			IMyClass anotherObj = context.Resolve<IMyClass>();
			Assert.AreSame(obj, anotherObj);

			IMyClass oneMorerObj = context.Resolve<IMyClass>();
			Assert.AreSame(obj, oneMorerObj);
		}

		[Test]
		public void TestwWithoutCustomFactoryWrapper ()
		{
			IDIContext context = ContextHelper.CreateContext();
			context.m().Bind<IMyClass>(() => new MyClass());

			IMyClass obj = context.Resolve<IMyClass> ();
			Assert.IsInstanceOf<MyClass>(obj);
		}


		[Test]
		public void TestCustomFactoryWrapper()
		{
			IDIContext context = ContextHelper.CreateContext();
			TestBinder testBinder = new TestBinder (context);
			testBinder.Bind<IMyClass>(() => new MyClass());

			IMyClass obj = context.Resolve<IMyClass>();

	//		Assert.IsInstanceOf<AnotherClass>(obj);
	//		Assert.IsInstanceOf<MyClass>((obj as AnotherClass).data);
		}
    }
}

