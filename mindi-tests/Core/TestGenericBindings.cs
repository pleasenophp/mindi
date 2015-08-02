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
			public IDIChainFactory<IOtherClass, IGlobalContextInitializer> chainFactory { get; set;}

		}

		[Test]
		public void TestGenericMPBinding() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().BindInstance<ContextEnvironment>(ContextEnvironment.Normal);
			context.m().BindGeneric(typeof(IDIFactory<>), typeof(ContextFactory<>));
			context.m().BindGeneric(typeof(IDIChainFactory<,>), typeof(ContextChainFactory<,>));

			context.m().Bind<IMyClass>(() => new MyClass());

			MyClass obj1 = context.Resolve<IMyClass, MyClass>();
			Assert.That(obj1.factory is ContextFactory<IOtherClass>);
			Assert.That(obj1.chainFactory is ContextChainFactory<IOtherClass, IGlobalContextInitializer>);

			MyClass obj2 = context.Resolve<IMyClass, MyClass>();
			Assert.That(obj2.factory is ContextFactory<IOtherClass>);
			Assert.That(obj2.chainFactory is ContextChainFactory<IOtherClass, IGlobalContextInitializer>);

			Assert.AreNotSame(obj1.factory, obj2.factory);
			Assert.AreNotSame(obj1.chainFactory, obj2.chainFactory);
		}

		[Test]
		public void TestGenericChainedBinding() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().BindInstance<ContextEnvironment>(ContextEnvironment.Normal);
			context.m().BindGeneric(typeof(IDIFactory<>), typeof(ContextFactory<>));
			context.m().BindGeneric(typeof(IDIChainFactory<,>), typeof(ContextChainFactory<,>));

			IDIContext childContext = ContextHelper.CreateContext(context);
			childContext.m().Bind<IMyClass>(() => new MyClass());

			MyClass obj1 = childContext.Resolve<IMyClass, MyClass>();
			Assert.That(obj1.factory is ContextFactory<IOtherClass>);
			Assert.That(obj1.chainFactory is ContextChainFactory<IOtherClass, IGlobalContextInitializer>);
		}

		// TODO - see why this doesn't work
		[Test]
		public void TestGenericRebinding() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().BindInstance<ContextEnvironment>(ContextEnvironment.Normal);
			context.m().BindGeneric(typeof(IDIFactory<>), typeof(ContextFactory<>));
			context.m().BindGeneric(typeof(IDIChainFactory<,>), typeof(ContextChainFactory<,>));

			IDIContext childContext = ContextHelper.CreateContext(context);
			childContext.m().Bind<IMyClass>(() => new MyClass());
			childContext.s().Rebind<IDIFactory<IMyClass>>();

			BindingDescriptor desc = childContext.Introspect<IDIFactory<IMyClass>>();
			Assert.AreEqual(BindingType.Factory, desc.bindingType);
			Assert.AreEqual(InstantiationType.Concrete, desc.instantiationType);
			Assert.AreEqual(childContext, desc.context);


			MyClass obj1 = childContext.Resolve<IMyClass, MyClass>();
			Assert.That(obj1.factory is ContextFactory<IOtherClass>);
			Assert.That(obj1.chainFactory is ContextChainFactory<IOtherClass, IGlobalContextInitializer>);

			MyClass obj2 = childContext.Resolve<IMyClass, MyClass>();
			Assert.That(obj2.factory is ContextFactory<IOtherClass>);
			Assert.That(obj2.chainFactory is ContextChainFactory<IOtherClass, IGlobalContextInitializer>);

			Assert.AreSame(obj1.factory, obj2.factory);
			Assert.AreNotSame(obj1.chainFactory, obj2.chainFactory);
		}

		// TODO - make it work when singletone generic binding is allowed
		/*
		[Test]
		public void TestGenericSBinding() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().BindInstance<ContextEnvironment>(ContextEnvironment.Normal);

			context.s().BindGeneric(typeof(IDIFactory<>), typeof(ContextFactory<>));
			context.s().BindGeneric(typeof(IDIChainFactory<,>), typeof(ContextChainFactory<,>));
			context.m().Bind<IMyClass>(() => new MyClass());

			MyClass obj1 = context.Resolve<IMyClass, MyClass>();
			Assert.That(obj1.factory is ContextFactory<IOtherClass>);
			Assert.That(obj1.chainFactory is ContextChainFactory<IOtherClass, IGlobalContextInitializer>);

			MyClass obj2 = context.Resolve<IMyClass, MyClass>();
			Assert.That(obj2.factory is ContextFactory<IOtherClass>);
			Assert.That(obj2.chainFactory is ContextChainFactory<IOtherClass, IGlobalContextInitializer>);

			Assert.AreSame(obj1.factory, obj2.factory);
			Assert.AreSame(obj1.chainFactory, obj2.chainFactory);

		}
		*/


    }
}

