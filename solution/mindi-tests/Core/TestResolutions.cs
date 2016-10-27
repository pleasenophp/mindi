using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using minioc.misc;
using minioc;

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

		private class MyClassMethod : ContextObject, IMyClass {
			public IOrange orange { get; set; }

			public IApple apple { get; set; }

			[Injection]
			protected void InjectionMethod(IOrange orange, IApple apple) {
				this.orange = orange;
				this.apple = apple;
			}

		}

		private class MyClassMethodMixed : ContextObject, IMyClass {
			[Injection]
			public IOrange orange { get; set; }

			public IApple apple { get; set; }

			[Injection]
			protected void InjectionMethod(IApple apple) {
				this.apple = apple;
			}
		}

		private class MyClassMethodSeveral : ContextObject, IMyClass {
			public IOrange orange { get; set; }

			public IApple apple { get; set; }

			[Injection]
			protected void InjectionMethod(IOrange orange) {
				this.orange = orange;
			}

			[SoftInjection]
			protected void InjectionMethod(IApple apple) {
				this.apple = apple;
			}
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

			IMyClass obj = context.Resolve<IMyClass>(() => Construction
				.ForType<IOrange>(new Orange()).AndType<IApple>(new Apple()));
			
			Assert.That(obj.orange is Orange);
			Assert.That(obj.apple is Apple);
		}

		[Test]
		public void TestConstructorContextRestricted() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().Bind<IMyClass>(()=>new MyClass());

			Assert.Catch<MiniocException>(() => {
				context.Resolve<IMyClass>(() => Construction
					.ForType<IOrange>(new Orange())
					.AndType<IApple>(new Apple())
					.AndType<IDIContext>(new MiniocContext())
				);	
			});
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

			IMyClass obj = context.Resolve<IMyClass>(() => Construction
				.ForType<IOrange>(new Orange()).AndType<IApple>(new Apple())
			);

			Assert.That(obj.orange is Orange);
			Assert.That(obj.apple is Apple);
		}

		[Test]
		public void TestTypelessRequirement() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().Bind<IMyClass>(()=>new MyClassRequirement());

			IMyClass obj = context.Resolve<IMyClass>(() => Construction
				.For("orange", new Orange())
				.And("apple", new Apple())
			);

			Assert.That(obj.orange is Orange);
			Assert.That(obj.apple is Apple);
		}


		[Test]
		public void TestSoftRequirement() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().Bind<IMyClass>(()=>new MyClassSoftRequirement());

			IMyClass obj = context.Resolve<IMyClass>(() => Construction
				.ForType<IApple>(new Apple())
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

			IMyClass obj = context.Resolve<IMyClass>(() => Construction
				.ForType<IApple>(new DefaultApple())
			);

			Assert.That(obj.orange is Orange);
			Assert.That(obj.apple is DefaultApple);
		}

		[Test]
		public void TestSeveralRequirementsSameType() {
			IDIContext context = ContextHelper.CreateContext();
			context.m().Bind<IMyOtherClass>(()=>new MyClassSameTypeRequirements());

			IMyOtherClass obj = context.Resolve<IMyOtherClass>(() => Construction
				.ForType<IOrange>(new Orange()).AndType<IApple>(new Apple())
			);

			Assert.That(obj.orange is Orange);
			Assert.That(obj.anotherOrange is Orange);
			Assert.AreSame(obj.orange, obj.anotherOrange);
			Assert.That(obj.apple is Apple);

			obj = context.Resolve<IMyOtherClass>(() => Construction
				.ForType<IOrange>("orange", new Orange()).AndType<IApple>(new Apple())
				.AndType<IOrange>("anotherOrange", new DefaultOrange())
			);

			Assert.That(obj.orange is Orange);
			Assert.That(obj.anotherOrange is DefaultOrange);
			Assert.That(obj.apple is Apple);
		}

		[Test]
		public void TestMethodInjection() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().Bind<IMyClass>(()=>new MyClassMethod());
			context.m().Bind<IOrange>(()=>new Orange());
			context.m().Bind<IApple>(()=>new Apple());

			IMyClass obj = context.Resolve<IMyClass>();
			Assert.That(obj.orange is Orange);
			Assert.That(obj.apple is Apple);
		}

		[Test]
		public void TestMixedInjection() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().Bind<IMyClass>(()=>new MyClassMethodMixed());
			context.m().Bind<IOrange>(()=>new Orange());
			context.m().Bind<IApple>(()=>new Apple());

			IMyClass obj = context.Resolve<IMyClass>();
			Assert.That(obj.orange is Orange);
			Assert.That(obj.apple is Apple);
		}

		[Test]
		public void TestSeveralMethodsInjection() {
			IDIContext context = ContextHelper.CreateContext();
			context.m().Bind<IMyClass>(()=>new MyClassMethodSeveral());
			context.m().Bind<IOrange>(()=>new Orange());

			IMyClass obj = context.Resolve<IMyClass>();
			Assert.That(obj.orange is Orange);
			Assert.That(obj.apple == null);

			obj = context.Resolve<IMyClass>(() => Construction.ForType<IApple>(new Apple()));
			Assert.That(obj.orange is Orange);
			Assert.That(obj.apple is Apple);

			context.m().Bind<IApple>(()=>new Apple());
			obj = context.Resolve<IMyClass>();
			Assert.That(obj.orange is Orange);
			Assert.That(obj.apple is Apple);
		}
    }
}

