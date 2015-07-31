using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using MinDI.Introspection;

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
			context.m().Bind<IMyClass>(()=>new MyClass());

			IDIContext childContext = ContextHelper.CreateContext(context);

			BindingDescriptor d2 = childContext.Introspect<IMyClass>();
			Assert.AreEqual(InstantiationType.Abstract, d2.instantiationType);
			Assert.AreEqual(context, d2.context);

			childContext.s().Rebind<IMyClass>();

			IMyClass a1 = context.Resolve<IMyClass>();
			IMyClass a2 = context.Resolve<IMyClass>();
			Assert.AreNotSame(a1, a2);

			IMyClass b1 = childContext.Resolve<IMyClass>();
			IMyClass b2 = childContext.Resolve<IMyClass>();
			Assert.AreSame(b1, b2);
			Assert.AreNotSame(b1, a1);

			BindingDescriptor d1 = context.Introspect<IMyClass>();
			Assert.AreEqual(InstantiationType.Abstract, d1.instantiationType);
			Assert.AreEqual(context, d1.context);

			d2 = childContext.Introspect<IMyClass>();
			Assert.AreEqual(InstantiationType.Concrete, d2.instantiationType);
			Assert.AreEqual(childContext, d2.context);
		}

		[Test]
		public void TestRebindSingletonToMultiple() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().Bind<IMyClass>(()=>new MyClass());

			IDIContext childContext = ContextHelper.CreateContext(context);
			childContext.m().Rebind<IMyClass>();

			IMyClass a1 = context.Resolve<IMyClass>();
			IMyClass a2 = context.Resolve<IMyClass>();
			Assert.AreSame(a1, a2);

			IMyClass b1 = childContext.Resolve<IMyClass>();
			IMyClass b2 = childContext.Resolve<IMyClass>();
			Assert.AreNotSame(b1, b2);
			Assert.AreNotSame(b1, a1);

			BindingDescriptor d1 = context.Introspect<IMyClass>();
			Assert.AreEqual(InstantiationType.Concrete, d1.instantiationType);
			Assert.AreEqual(context, d1.context);

			BindingDescriptor d2 = childContext.Introspect<IMyClass>();
			Assert.AreEqual(InstantiationType.Abstract, d2.instantiationType);
			Assert.AreEqual(childContext, d2.context);
		}

		[Test]
		public void TestRebindSingletonToSingleton() {
			IDIContext context = ContextHelper.CreateContext();

			context.s().Bind<IMyClass>(()=>new MyClass());

			IDIContext childContext = ContextHelper.CreateContext(context);
			childContext.s().Rebind<IMyClass>();

			IMyClass a1 = context.Resolve<IMyClass>();
			IMyClass a2 = context.Resolve<IMyClass>();
			Assert.AreSame(a1, a2);

			IMyClass b1 = childContext.Resolve<IMyClass>();
			IMyClass b2 = childContext.Resolve<IMyClass>();
			Assert.AreSame(b1, b2);
			Assert.AreNotSame(b1, a1);
			Assert.AreNotSame(b2, a2);


			BindingDescriptor d1 = context.Introspect<IMyClass>();
			Assert.AreEqual(InstantiationType.Concrete, d1.instantiationType);
			Assert.AreEqual(context, d1.context);

			BindingDescriptor d2 = childContext.Introspect<IMyClass>();
			Assert.AreEqual(InstantiationType.Concrete, d2.instantiationType);
			Assert.AreEqual(childContext, d2.context);
		}

	
    }
}

