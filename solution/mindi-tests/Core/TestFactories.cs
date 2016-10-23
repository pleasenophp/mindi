using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using minioc.misc;
using MinDI.StateObjects;

namespace MinDI.Tests
{
    [TestFixture]
    [Category("Unit tests")]
    internal class TestFactories {
				
		private interface IOrange {}

		private interface IApple {}

		private interface IMyClass {
			IOrange orange { get; }
			IApple apple { get; }
		}

		private interface IMyOtherClass : IMyClass {
			IOrange anotherOrange { get; }
		}

		private class Orange : IOrange {
		}

		private class Apple : IApple {
		}

		private class DefaultApple : IApple {
		}

		private class DefaultOrange : IOrange {
		}
			
		private class MyClass : ContextObject, IMyClass {
			[Injection]
			public IOrange orange { get; set; }

			[Injection]
			public IApple apple { get; set; }
		}

		private class MyClassRequirement : ContextObject, IMyClass {
			[Requirement]
			public IOrange orange { get; set; }

			[Requirement]
			public IApple apple { get; set; }
		}


		private class MyClassFactory : BaseContextFactory<IMyClass> {
			public IMyClass Create() {
				IMyClass myClass = base.CreateInstance();
				return myClass;
			}
		}

		private class MyClassConstructorFactory : BaseContextFactory<IMyClass> {
			public IMyClass Create(IApple apple, IOrange orange) {
				IMyClass myClass = base.CreateInstance(() => Construction.
					ForType<IApple>(apple)
					.AndType<IOrange>(orange)
				);
				return myClass;
			}
		}
			
		[Test]
		public void TestCustomFactory() {
			IDIContext context = ContextHelper.CreateContext<IGlobalContextInitializer>();
			context.m().Bind(() => new MyClassFactory());
			context.m().Bind<IApple>(()=>new Apple());
			context.m().Bind<IOrange>(()=>new Orange());
			context.m().Bind<IMyClass>(()=>new MyClass());

			var factory = context.Resolve<MyClassFactory>();

			IMyClass instance = factory.Create();
			Assert.IsNotNull(instance);

			instance = factory.Destroy(instance);
			Assert.IsNull(instance);
		}

		[Test]
		public void TestConstructionFactory() {
			IDIContext context = ContextHelper.CreateContext<IGlobalContextInitializer>();
			context.m().Bind(() => new MyClassConstructorFactory());
			context.m().Bind<IMyClass>(()=>new MyClass());

			var factory = context.Resolve<MyClassConstructorFactory>();

			IMyClass instance = factory.Create(new Apple(), new Orange());
			Assert.IsNotNull(instance);
			IDIClosedContext cto = instance as IDIClosedContext;
			Assert.AreSame(cto.descriptor.context, context);

			instance = factory.Destroy(instance);
			Assert.IsNull(instance);
		}


    }
}

