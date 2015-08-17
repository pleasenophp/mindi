using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using minioc;
using minioc.context.bindings;
using minioc.misc;

namespace MinDI.Tests.MinIOC
{
    [TestFixture]
    [Category("Other")]
    internal class MinIOC
    {
		interface IApple {
			void TestContext(IDIContext context);
		}

		interface IOrange {
			string tag {get; set;}
		}

		class RedOrange : IOrange {
			public string tag {get; set;}
		}

		class BlueOrange : IOrange {
			public string tag {get; set;}
		}

		class GreenApple : ContextObject, IApple {
			[Injection]
			public IDIContext testContext {get; set;}

			public void TestContext(IDIContext cont) {
				Assert.AreSame(cont, testContext);
			}

		}
		
		class YellowApple : GreenApple, IApple {
		}

		interface IModelWrite {
		}

		interface IModelRead {
		}

		class Model : IModelRead, IModelWrite {
		}

		[Test]
		public void MultipleSameContextTest()
		{
			IDIContext context = new MiniocContext();

			context.Register(Bindings
				.ForType<IOrange>().ImplementedBy(() => new RedOrange()));
			                 
			IOrange orange1 = context.Resolve<IOrange>();
			IOrange orange2 = context.Resolve<IOrange>();
			
			Assert.That(orange1 is RedOrange);
			Assert.That(orange2 is RedOrange);
			
			Assert.AreNotSame(orange1, orange2);
		}

        [Test]
        public void SingletoneSameContextTest()
        {
			MiniocContext context = new MiniocContext();
			context.s().Bind<IOrange>(() => new RedOrange());

			IOrange orange1 = context.Resolve<IOrange>();
			IOrange orange2 = context.Resolve<IOrange>();

			Assert.That(orange1 is RedOrange);
			Assert.That(orange2 is RedOrange);

			Assert.AreSame(orange1, orange2);
        }

		[Test]
		public void SingletoneFactorySameContextTest() {
			IDIContext context = new MiniocContext();

			context.s().BindInstance<IDIContext>(context);
			context.s().Bind<IApple>(()=>new GreenApple());

			IApple apple1 = context.Resolve<IApple>();
			IApple apple2 = context.Resolve<IApple>();
			
			Assert.That(apple1 is GreenApple);
			Assert.That(apple2 is GreenApple);
			
			Assert.AreSame(apple1, apple2);
		}

		[Test]
		public void SingletoneDifferentContextTest()
		{
			MiniocContext context1 = new MiniocContext();

			context1.s().Bind<IOrange>(() => new RedOrange());

			MiniocContext context2 = new MiniocContext();
		
			context2.s().Bind<IOrange>(() => new RedOrange());
			
			
			IOrange orange1 = context1.Resolve<IOrange>();
			IOrange orange2 = context2.Resolve<IOrange>();
			
			Assert.That(orange1 is RedOrange);
			Assert.That(orange2 is RedOrange);
			
			Assert.AreNotSame(orange1, orange2);
		}

		[Test]
		public void SingletoneFactoryDifferentContextTest()
		{
			IDIContext context1 = new MiniocContext();

			context1.s().Bind<IOrange>(() => new RedOrange());
			
			IDIContext context2 = new MiniocContext();
			context2.s().Bind<IOrange>(() => new RedOrange());

			IOrange orange1 = context1.Resolve<IOrange>();
			IOrange orange2 = context2.Resolve<IOrange>();
			
			Assert.That(orange1 is RedOrange);
			Assert.That(orange2 is RedOrange);
			
			Assert.AreNotSame(orange1, orange2);
		}

		[Test]
		public void SingletoneChainedContextTest()
		{
			MiniocContext context1 = new MiniocContext();


			context1.s().Bind<IOrange>(() => new RedOrange());
			
			MiniocContext context2 = new MiniocContext(context1);

			IOrange orange1 = context1.Resolve<IOrange>();
			IOrange orange2 = context2.Resolve<IOrange>();
			
			Assert.That(orange1 is RedOrange);
			Assert.That(orange2 is RedOrange);
			
			Assert.AreSame(orange1, orange2);

			context2.s().Bind<IOrange>(() => new RedOrange());

			orange2 = context2.Resolve<IOrange>();
			Assert.That(orange2 is RedOrange);
			Assert.AreNotSame(orange1, orange2);
		}

		[Test]
		public void TestContextSelf() {
			IDIContext context = new MiniocContext();

			context.s().BindInstance<IDIContext>(context);
			context.s().Bind<IApple>(() => new YellowApple());

			IApple apple = context.Resolve<IApple>();
			Assert.That(apple is YellowApple);
			apple.TestContext(context);

		}

		[Test]
		public void TestSingletonPersistance() {
			MiniocContext context = new MiniocContext();
		
			context.s().Bind<IOrange>(() => new BlueOrange());

			SingletoneCreation(context);

			IOrange orange = context.Resolve<IOrange>();
			Assert.AreEqual("my_test_orange", orange.tag);
		}

		private void SingletoneCreation(MiniocContext context) {
			IOrange orange = context.Resolve<IOrange>();
			Assert.That(orange is BlueOrange);
			orange.tag = "my_test_orange";
		}


		[Test]
		public void TestNotBindedException()
		{
			MiniocContext context = new MiniocContext();

			Assert.Throws<MiniocException>(() => {
				context.Resolve<IOrange>();	
			});

			MiniocContext childContext = new MiniocContext(context);
			Assert.Throws<MiniocException>(() => {
				childContext.Resolve<IOrange>();	
			});

		}
    }
}

