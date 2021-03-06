using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using MinDI.Context;
using MinDI.Introspection;

namespace MinDI.Tests
{
    [TestFixture]
    [Category("Unit tests")]
    internal class TestGenericBindings {

		private interface IMyClass {}

		private interface IOtherClass {}

		private class MyClass : ContextObject, IMyClass {
			[Injection]
			public IDIFactory<IOtherClass> factory { get; set;}

			[Injection]
			public IDIRFactory<IOtherClass, IGlobalContextInitializer> chainFactory { get; set;}

		}

		private class TestClassMany : ContextObject
		{
			[Injection]
			public ICollection<int> collection1 { get; set; }

			[Injection]
			public IList<int> list1 { get; set; }

			[Injection]
			public IList<string> list2 { get; set; }

			[Injection]
			public IList<string> collection2 { get; set; }
		}



		[Test]
		public void TestGenericMPBinding() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().BindInstance<ContextEnvironment>(ContextEnvironment.Normal);
			context.m().BindGeneric(typeof(IDIFactory<>), typeof(ContextFactory<>));
			context.m().BindGeneric(typeof(IDIRFactory<,>), typeof(ReproduceContextFactory<,>));

			context.m().Bind<IMyClass>(() => new MyClass());

			MyClass obj1 = context.Resolve<IMyClass>() as MyClass;
			Assert.That(obj1.factory is ContextFactory<IOtherClass>);
			Assert.That(obj1.chainFactory is ReproduceContextFactory<IOtherClass, IGlobalContextInitializer>);

			MyClass obj2 = context.Resolve<IMyClass>() as MyClass;
			Assert.That(obj2.factory is ContextFactory<IOtherClass>);
			Assert.That(obj2.chainFactory is ReproduceContextFactory<IOtherClass, IGlobalContextInitializer>);

			Assert.AreNotSame(obj1.factory, obj2.factory);
			Assert.AreNotSame(obj1.chainFactory, obj2.chainFactory);
		}

		[Test]
		public void TestGenericMPBindingMany ()
		{
			IDIContext context = ContextHelper.CreateContext ();
			context.m().BindGenericMany (new List<Type> {typeof(ICollection<>), typeof(IList<>)}, typeof(List<>));
			context.m().Bind (() => new TestClassMany());

			var obj1 = context.Resolve<TestClassMany>();
			Assert.That(obj1.list1 is List<int>);
			Assert.That(obj1.list2 is List<string>);
			Assert.That(obj1.collection1 is List<int>);

			Assert.AreNotSame(obj1.list1, obj1.collection1);
			Assert.AreNotSame(obj1.list2, obj1.collection1);
			Assert.AreNotSame(obj1.list2, obj1.list1);
		}

		[Test]
		public void TestGenericSBindingMany ()
		{
			IDIContext context = ContextHelper.CreateContext ();
			context.s ().BindGenericMany (new List<Type> { typeof (ICollection<>), typeof (IList<>) }, typeof (List<>));
			context.m ().Bind (() => new TestClassMany ());

			var obj1 = context.Resolve<TestClassMany> ();
			Assert.That (obj1.list1 is List<int>);
			Assert.That (obj1.list2 is List<string>);
			Assert.That (obj1.collection1 is List<int>);
			Assert.That (obj1.collection2 is List<string>);

			Assert.AreSame (obj1.list1, obj1.collection1);
			Assert.AreSame (obj1.list2, obj1.collection2);
			Assert.AreNotSame (obj1.list2, obj1.collection1);
			Assert.AreNotSame (obj1.list2, obj1.list1);
			Assert.AreNotSame (obj1.collection2, obj1.collection1);
		}

		[Test]
		public void TestGenericChainedBinding() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().BindInstance<ContextEnvironment>(ContextEnvironment.Normal);
			context.m().BindGeneric(typeof(IDIFactory<>), typeof(ContextFactory<>));
			context.m().BindGeneric(typeof(IDIRFactory<,>), typeof(ReproduceContextFactory<,>));

			IDIContext childContext = ContextHelper.CreateContext(context);
			childContext.m().Bind<IMyClass>(() => new MyClass());

			MyClass obj1 = childContext.Resolve<IMyClass>() as MyClass;
			Assert.That(obj1.factory is ContextFactory<IOtherClass>);
			Assert.That(obj1.chainFactory is ReproduceContextFactory<IOtherClass, IGlobalContextInitializer>);
		}
			
		[Test]
		public void TestGenericRebinding() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().BindInstance<ContextEnvironment>(ContextEnvironment.Normal);
			context.m().BindGeneric(typeof(IDIFactory<>), typeof(ContextFactory<>));
			context.m().BindGeneric(typeof(IDIRFactory<,>), typeof(ReproduceContextFactory<,>));

			IDIContext childContext = ContextHelper.CreateContext(context);
			childContext.m().Bind<IMyClass>(() => new MyClass());
			childContext.s().Rebind<IDIFactory<IOtherClass>>();

			MyClass obj1 = childContext.Resolve<IMyClass>() as MyClass;
			Assert.That(obj1.factory is ContextFactory<IOtherClass>);
			Assert.That(obj1.chainFactory is ReproduceContextFactory<IOtherClass, IGlobalContextInitializer>);

			MyClass obj2 = childContext.Resolve<IMyClass>() as MyClass;
			Assert.That(obj2.factory is ContextFactory<IOtherClass>);
			Assert.That(obj2.chainFactory is ReproduceContextFactory<IOtherClass, IGlobalContextInitializer>);

			Assert.AreSame(obj1.factory, obj2.factory);
			Assert.AreNotSame(obj1.chainFactory, obj2.chainFactory);
		}

		[Test]
		public void TestGenericIntrospect() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().BindInstance<ContextEnvironment>(ContextEnvironment.Normal);
			context.m().BindGeneric(typeof(IDIFactory<>), typeof(ContextFactory<>));
			context.m().BindGeneric(typeof(IDIRFactory<,>), typeof(ReproduceContextFactory<,>));

			IDIContext childContext = ContextHelper.CreateContext(context);
			childContext.m().Bind<IMyClass>(() => new MyClass());
			childContext.s().Rebind<IDIFactory<IOtherClass>>();

			IBinding desc1 = childContext.Introspect<IDIFactory<IOtherClass>>();
			Assert.AreEqual(InstantiationType.Concrete, desc1.instantiationType);
			Assert.AreEqual(childContext, desc1.context);

			IBinding desc2 = childContext.Introspect<IDIRFactory<IOtherClass, IGlobalContextInitializer>>();
			Assert.AreEqual (InstantiationType.Abstract, desc2.instantiationType);
			Assert.AreEqual (context, desc2.context);
		}
			
		[Test]
		public void TestGenericSBinding() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().BindInstance<ContextEnvironment>(ContextEnvironment.Normal);

			context.s().BindGeneric(typeof(IDIFactory<>), typeof(ContextFactory<>));
			context.s().BindGeneric(typeof(IDIRFactory<,>), typeof(ReproduceContextFactory<,>));
			context.m().Bind<IMyClass>(() => new MyClass());

			MyClass obj1 = context.Resolve<IMyClass>() as MyClass;
			Assert.That(obj1.factory is ContextFactory<IOtherClass>);
			Assert.That(obj1.chainFactory is ReproduceContextFactory<IOtherClass, IGlobalContextInitializer>);

			MyClass obj2 = context.Resolve<IMyClass>() as MyClass;
			Assert.That(obj2.factory is ContextFactory<IOtherClass>);
			Assert.That(obj2.chainFactory is ReproduceContextFactory<IOtherClass, IGlobalContextInitializer>);

			Assert.AreSame(obj1.factory, obj2.factory);
			Assert.AreSame(obj1.chainFactory, obj2.chainFactory);

		}



    }
}

