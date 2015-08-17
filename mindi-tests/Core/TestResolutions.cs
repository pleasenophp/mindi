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

			[SoftInjection]
			public IApple apple { get; set; }
		}

		private class MyClassRequirement : ContextObject, IMyClass {
			[Requirement]
			public IOrange orange { get; set; }

			[Requirement]
			public IApple apple { get; set; }
		}

		private class MyClassSameTypeRequirements : ContextObject, IMyOtherClass {
			[Requirement]
			public IOrange orange { get; set; }

			[Requirement]
			public IOrange anotherOrange { get; set; }

			[Requirement]
			public IApple apple { get; set; }
		}

		private class MyClassSoftRequirement : ContextObject, IMyClass {
			[SoftRequirement]
			public IOrange orange { get; set; }

			[SoftRequirement]
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

		[Test]
		public void TestConstructorResolution() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().Bind<IMyClass>(()=>new MyClass());

			IMyClass obj = context.Resolve<IMyClass>(
				Construction.For<IOrange>(new Orange()).And<IApple>(new Apple()));
			
			Assert.That(obj.orange is Orange);
			Assert.That(obj.apple is Apple);
		}

		[Test]
		public void TestRequirementError() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().Bind<IMyClass>(()=>new MyClassRequirement());

			Assert.Throws<MiniocException>(() => {
				context.Resolve<IMyClass>();
			});
		}

		[Test]
		public void TestRequirement() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().Bind<IMyClass>(()=>new MyClassRequirement());

			IMyClass obj = context.Resolve<IMyClass>(
				Construction.For<IOrange>(new Orange()).And<IApple>(new Apple())
			);

			Assert.That(obj.orange is Orange);
			Assert.That(obj.apple is Apple);
		}

		[Test]
		public void TestSoftRequirement() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().Bind<IMyClass>(()=>new MyClassSoftRequirement());

			IMyClass obj = context.Resolve<IMyClass>(
				Construction.For<IApple>(new Apple())
			);
				
			Assert.That(obj.apple is Apple);
			Assert.IsNull(obj.orange);
		}

		[Test]
		public void TestRequirementPriorityOverContext() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().Bind<IMyClass>(()=>new MyClass());
			context.m().Bind<IOrange>(()=>new Orange());
			context.m().Bind<IApple>(()=>new Apple());

			IMyClass obj = context.Resolve<IMyClass>(
				Construction.For<IApple>(new DefaultApple())
			);

			Assert.That(obj.orange is Orange);
			Assert.That(obj.apple is DefaultApple);
		}

		[Test]
		public void TestSeveralRequirementsSameType() {
			IDIContext context = ContextHelper.CreateContext();
			context.m().Bind<IMyOtherClass>(()=>new MyClassSameTypeRequirements());

			IMyOtherClass obj = context.Resolve<IMyOtherClass>(
				Construction.For<IOrange>(new Orange()).And<IApple>(new Apple())
			);

			Assert.That(obj.orange is Orange);
			Assert.That(obj.anotherOrange is Orange);
			Assert.AreSame(obj.orange, obj.anotherOrange);
			Assert.That(obj.apple is Apple);

			obj = context.Resolve<IMyOtherClass>(
				Construction.For<IOrange>(new Orange(), "orange").And<IApple>(new Apple())
				.And<IOrange>(new DefaultOrange(), "anotherOrange")
			);

			Assert.That(obj.orange is Orange);
			Assert.That(obj.anotherOrange is DefaultOrange);
			Assert.That(obj.apple is Apple);
		}
    }
}

