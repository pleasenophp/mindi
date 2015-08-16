using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using minioc.misc;

namespace MinDI.Tests
{
    [TestFixture]
    [Category("Unit tests")]
    internal class TestResolutions {

		
		private interface IOrange {}

		private interface IApple {}

		private interface IMyClass {
			IOrange orange { get; }
			IApple apple { get; }
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

			[SoftInjection]
			public IApple apple { get; set; }
		}

		private class MyClassWithDefault : ContextObject, IMyClass {
			public MyClassWithDefault() {
				orange = new DefaultOrange();
				apple = new DefaultApple();
			}

			[SoftInjection]
			public IOrange orange { get; set; }

			[SoftInjection]
			public IApple apple { get; set; }
		}

		[Test]
		public void TestSimpleResolution() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().Bind<IMyClass>(()=>new MyClass());
			context.m().Bind<IOrange>(()=>new Orange());
			context.m().Bind<IApple>(()=>new Apple());

			IMyClass obj = context.Resolve<IMyClass>();
			Assert.That(obj.orange is Orange);
			Assert.That(obj.apple is Apple);
		}

		[Test]
		public void TestSimpleResolutionException() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().Bind<IMyClass>(()=>new MyClass());
			context.m().Bind<IApple>(()=>new Apple());

			Assert.Catch<MiniocException>(() => {
				context.Resolve<IMyClass>();
			});
		}

		[Test]
		public void TestSoftResolution() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().Bind<IMyClass>(()=>new MyClass());
			context.m().Bind<IOrange>(()=>new Orange());

			IMyClass obj = context.Resolve<IMyClass>();
			Assert.That(obj.orange is Orange);
			Assert.IsNull(obj.apple);
		}

		[Test]
		public void TestSoftResolutionWithDefault() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().Bind<IMyClass>(()=>new MyClassWithDefault());

			IMyClass obj = context.Resolve<IMyClass>();
			Assert.That(obj.orange is DefaultOrange);
			Assert.That(obj.apple is DefaultApple);
		}

    }
}

