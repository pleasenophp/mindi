using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

namespace MinDI.Tests
{
    [TestFixture]
    [Category("Unit tests")]
    internal class TestRebinding {

		private interface IMyClass {}

		private class MyClass : IMyClass {
		}

		[Test]
		public void TestRebindMultipleToSingleton() {
			IDIContext context = ContextHelper.CreateContext();
			var bind = context.CreateBindHelper();
			bind.multiple.Bind<IMyClass>(()=>new MyClass());

			IDIContext childContext = ContextHelper.CreateContext(context);
			bind = childContext.CreateBindHelper();
			bind.singleton.Rebind<IMyClass>();

			IMyClass a1 = context.Resolve<IMyClass>();
			IMyClass a2 = context.Resolve<IMyClass>();
			Assert.AreNotSame(a1, a2);

			IMyClass b1 = childContext.Resolve<IMyClass>();
			IMyClass b2 = childContext.Resolve<IMyClass>();
			Assert.AreSame(b1, b2);
			Assert.AreNotSame(b1, a1);
		}

		[Test]
		public void TestRebindSingletonToMultiple() {
			IDIContext context = ContextHelper.CreateContext();
			var bind = context.CreateBindHelper();
			bind.singleton.Bind<IMyClass>(()=>new MyClass());

			IDIContext childContext = ContextHelper.CreateContext(context);
			bind = childContext.CreateBindHelper();
			bind.multiple.Rebind<IMyClass>();

			IMyClass a1 = context.Resolve<IMyClass>();
			IMyClass a2 = context.Resolve<IMyClass>();
			Assert.AreSame(a1, a2);

			IMyClass b1 = childContext.Resolve<IMyClass>();
			IMyClass b2 = childContext.Resolve<IMyClass>();
			Assert.AreNotSame(b1, b2);
			Assert.AreNotSame(b1, a1);
		}

		[Test]
		public void TestRebindSingletonToSingleton() {
			IDIContext context = ContextHelper.CreateContext();
			var bind = context.CreateBindHelper();
			bind.singleton.Bind<IMyClass>(()=>new MyClass());

			IDIContext childContext = ContextHelper.CreateContext(context);
			bind = childContext.CreateBindHelper();
			bind.singleton.Rebind<IMyClass>();

			IMyClass a1 = context.Resolve<IMyClass>();
			IMyClass a2 = context.Resolve<IMyClass>();
			Assert.AreSame(a1, a2);

			IMyClass b1 = childContext.Resolve<IMyClass>();
			IMyClass b2 = childContext.Resolve<IMyClass>();
			Assert.AreSame(b1, b2);
			Assert.AreNotSame(b1, a1);
			Assert.AreNotSame(b2, a2);
		}

	
    }
}

