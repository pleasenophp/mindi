using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using minioc;
using minioc.context.bindings;

namespace MinDI.Tests.MinIOC
{
    [TestFixture]
    [Category("Other")]
    internal class MinIOC
    {
		interface IApple {
			void TestContext(MiniocContext context);
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

		class GreenApple : IApple {
			[Injection]
			public MiniocContext context {get; set;}

			public void TestContext(MiniocContext cont) {
				Assert.AreSame(context, cont);
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
			MiniocContext context = new MiniocContext();
			
			context.Register(Bindings
			                 .ForType<IOrange>()
			                 .ImplementedBy<RedOrange>()
			                 .SetInstantiationMode(InstantiationMode.MULTIPLE)
			                 );
			
			
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

			context.Register(Bindings
			                 .ForType<IOrange>()
			                 .ImplementedBy<RedOrange>()
			                 .SetInstantiationMode(InstantiationMode.SINGLETON)
			                 );


			IOrange orange1 = context.Resolve<IOrange>();
			IOrange orange2 = context.Resolve<IOrange>();

			Assert.That(orange1 is RedOrange);
			Assert.That(orange2 is RedOrange);

			Assert.AreSame(orange1, orange2);
        }

		[Test]
		public void SingletoneFactorySameContextTest() {
			MiniocContext context = new MiniocContext();

			context.Register(Bindings
			                 .ForType<MiniocContext>()
			                 .ImplementedByInstance(context)
			                 .SetInstantiationMode(InstantiationMode.SINGLETON)
			                 );

			context.Register(Bindings
			                 .ForType<IApple>()
			                 .ImplementedBy(()=>new GreenApple())
			                 .SetInstantiationMode(InstantiationMode.SINGLETON)
			                 );
			
			
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
			
			context1.Register(Bindings
			                 .ForType<IOrange>()
			                 .ImplementedBy<RedOrange>()
			                 .SetInstantiationMode(InstantiationMode.SINGLETON)
			                 );

			MiniocContext context2 = new MiniocContext();
			
			context2.Register(Bindings
			                 .ForType<IOrange>()
			                 .ImplementedBy<RedOrange>()
			                 .SetInstantiationMode(InstantiationMode.SINGLETON)
			                 );
			
			
			IOrange orange1 = context1.Resolve<IOrange>();
			IOrange orange2 = context2.Resolve<IOrange>();
			
			Assert.That(orange1 is RedOrange);
			Assert.That(orange2 is RedOrange);
			
			Assert.AreNotSame(orange1, orange2);
		}

		[Test]
		public void SingletoneFactoryDifferentContextTest()
		{
			MiniocContext context1 = new MiniocContext();
			
			context1.Register(Bindings
			                  .ForType<IOrange>()
			                  .ImplementedBy(()=>new RedOrange())
			                  .SetInstantiationMode(InstantiationMode.SINGLETON)
			                  );
			
			MiniocContext context2 = new MiniocContext();
			
			context2.Register(Bindings
			                  .ForType<IOrange>()
			                  .ImplementedBy(()=>new RedOrange())
			                  .SetInstantiationMode(InstantiationMode.SINGLETON)
			                  );
			
			
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
			
			context1.Register(Bindings
			                  .ForType<IOrange>()
			                  .ImplementedBy<RedOrange>()
			                  .SetInstantiationMode(InstantiationMode.SINGLETON)
			                  );
			
			MiniocContext context2 = new MiniocContext(context1);

			IOrange orange1 = context1.Resolve<IOrange>();
			IOrange orange2 = context2.Resolve<IOrange>();
			
			Assert.That(orange1 is RedOrange);
			Assert.That(orange2 is RedOrange);
			
			Assert.AreSame(orange1, orange2);

			context2.Register(Bindings
			                  .ForType<IOrange>()
			                  .ImplementedBy(()=>new RedOrange())
			                  .SetInstantiationMode(InstantiationMode.SINGLETON)
			                  );

			orange2 = context2.Resolve<IOrange>();
			Assert.That(orange2 is RedOrange);
			Assert.AreNotSame(orange1, orange2);
		}

		[Test]
		public void TestContextSelf() {
			MiniocContext context = new MiniocContext();
			
			context.Register(Bindings
			                 .ForType<MiniocContext>()
			                 .ImplementedByInstance(context)
			                 .SetInstantiationMode(InstantiationMode.SINGLETON)
			                 );


			context.Register(Bindings
			                 .ForType<IApple>()
			                 .ImplementedBy<YellowApple>()
			                 .SetInstantiationMode(InstantiationMode.SINGLETON)
			                 );

			IApple apple = context.Resolve<IApple>();
			Assert.That(apple is YellowApple);
			apple.TestContext(context);

		}

		[Test]
		public void TestSingletonPersistance() {
			MiniocContext context = new MiniocContext();
			context.Register(Bindings
			                 .ForType<IOrange>()
			                 .ImplementedBy<BlueOrange>()
			                 .SetInstantiationMode(InstantiationMode.SINGLETON)
			                 );

			SingletoneCreation(context);

			IOrange orange = context.Resolve<IOrange>();
			Assert.AreEqual("my_test_orange", orange.tag);
		}

		private void SingletoneCreation(MiniocContext context) {
			IOrange orange = context.Resolve<IOrange>();
			Assert.That(orange is BlueOrange);
			orange.tag = "my_test_orange";
		}

		// This is a limitation that might be fixed in future version
		[Test]
		public void Test2InterfacesBindingLimitation() {
			MiniocContext context = new MiniocContext();
			
			context.Register(Bindings
			                 .ForType<IModelRead>()
			                 .ImplementedBy<Model>()
			                 .SetInstantiationMode(InstantiationMode.SINGLETON)
			                 );
			
			context.Register(Bindings
			                 .ForType<IModelWrite>()
			                 .ImplementedBy<Model>()
			                 .SetInstantiationMode(InstantiationMode.SINGLETON)
			                 );
			
			IModelRead read = context.Resolve<IModelRead>();
			IModelWrite write = context.Resolve<IModelWrite>();
			
			Assert.That(read is Model);
			Assert.That(write is Model);
			
			// When fixed it must be the same
			Assert.AreNotSame(read, write);			
		}
    }
}

