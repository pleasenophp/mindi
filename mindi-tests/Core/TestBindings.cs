using System;
using System.Collections.Generic;
using System.Threading;
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

		private interface IMyClass {}

		private class MyClass : IMyClass {
		}

		[Test]
		public void TestValueInstanceBinding() {
			IDIContext context = ContextHelper.CreateContext();
			var bind = context.CreateBindHelper();
			bind.singleton.BindInstance<int>(5);

			int i = context.Resolve<int>();
			Assert.AreEqual(5, i);

			bind.singleton.BindInstance<MyEnum>(MyEnum.Viktoria);
			MyEnum name = context.Resolve<MyEnum>();
			Assert.AreEqual(MyEnum.Viktoria, name);
		}

		[Test]
		public void TestInstanceBindingST() {
			MyClass obj = new MyClass();

			IDIContext context = ContextHelper.CreateContext();
			var bind = context.CreateBindHelper();
			bind.singleton.BindInstance<IMyClass>(obj);

			IMyClass anotherObj = context.Resolve<IMyClass>();
			Assert.AreSame(obj, anotherObj);
		}

		[Test]
		public void TestInstanceBindingMP() {
			MyClass obj = new MyClass();

			IDIContext context = ContextHelper.CreateContext();
			var bind = context.CreateBindHelper();
			bind.multiple.BindInstance<IMyClass>(obj);

			IMyClass anotherObj = context.Resolve<IMyClass>();
			Assert.AreSame(obj, anotherObj);

			IMyClass oneMorerObj = context.Resolve<IMyClass>();
			Assert.AreSame(obj, oneMorerObj);
		}
    }
}

