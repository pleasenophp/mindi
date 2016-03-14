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
    internal class Regression {

		private interface IMyClass {}

		private class MyClass : IMyClass {
		}

		interface IApple {
		}

		interface ISeed {
		}

		interface ISkin {
		}

		class Apple : IApple {

			[SoftRequirement] public ISkin skin { get; set;}
			[SoftRequirement] public ISeed seed { get; set;}

		}

		class BigApple : IApple {
		}

		class Seed : ISeed {
			
		}

		class Skin: ISkin {
		}


		[Test]
		public void TestNullNameResolve() {
			IDIContext context = ContextHelper.CreateContext();

			context.m().Bind<IApple>(() => new Apple());

			IApple appleInstance = context.Resolve<IApple>(null);
			Assert.IsNotNull(appleInstance);

			appleInstance = context.Resolve<IApple>("");
			Assert.IsNotNull(appleInstance);
		}

		[Test]
		public void TestNamedParentResolution() {
			IDIContext parentContext = ContextHelper.CreateContext();
			parentContext.m().Bind<IApple>(() => new Apple(), BindingName.ForType<Apple>());

			IDIContext context = parentContext.Reproduce();
			context.m().Bind<IApple>(() => new BigApple(), BindingName.ForType<BigApple>());

			IApple apple = context.Resolve<IApple>(BindingName.ForType<BigApple>());
			Assert.That(apple is BigApple);

			apple = context.Resolve<IApple>();
			Assert.That(apple is BigApple);

			apple = context.Resolve<IApple>(BindingName.ForType<Apple>());
			Assert.That(apple is Apple);
		}


		[Test]
		public void TestNamedParentIntrospection() {
			IDIContext parentContext = ContextHelper.CreateContext();
			parentContext.m().Bind<IApple>(() => new Apple(), BindingName.ForType<Apple>());

			IDIContext context = parentContext.Reproduce();
			context.m().Bind<IApple>(() => new BigApple(), BindingName.ForType<BigApple>());

			BindingDescriptor desc = context.Introspect<IApple>(BindingName.ForType<BigApple>());
			Assert.IsNotNull(desc);
			Assert.AreEqual(InstantiationType.Abstract, desc.instantiationType);
			Assert.That(desc.context == context);
			object apple = desc.factory();
			Assert.That(apple is BigApple);

			desc = context.Introspect<IApple>();
			Assert.IsNotNull(desc);
			Assert.AreEqual(InstantiationType.Abstract, desc.instantiationType);
			Assert.That(desc.context == context);
			apple = desc.factory();
			Assert.That(apple is BigApple);

			desc = context.Introspect<IApple>(BindingName.ForType<Apple>());
			Assert.IsNotNull(desc);
			Assert.AreEqual(InstantiationType.Abstract, desc.instantiationType);
			Assert.That(desc.context == parentContext);
			apple = desc.factory();
			Assert.That(apple is Apple);
		}

		[Test]
		public void TestNullBinding() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().BindInstance<IApple>(null);

			IApple apple = context.TryResolve<IApple>();
			Assert.IsNull(apple);
		}

    }
}

