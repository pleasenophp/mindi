using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using minioc;
using minioc.context.bindings;
using minioc.attributes;
using MinDI.Binders;

namespace MinDI.Tests
{

	[TestFixture]
    [Category("Other")]
    internal class MinIOCAdvancedTests
	{

		[Test]
		public void TestCreationByNew_IDICOntainerBranch() {
			MiniocContext context = ContextHelper.CreateContext();

			// Let's bind a test type using a usual new
			context.Register(Bindings
			                 .ForType<IDependencyTest>()
			                 .ImplementedBy(()=>new DependencyTest())
			                 .SetInstantiationMode(InstantiationMode.SINGLETON)
			                 );

			// Let's see that the dependencies are now injected - we have changed MinIOC. In original version this would fail
			IDependencyTest test = context.Resolve<IDependencyTest>();

			//Assert.IsNull(test.context); -- in original MinIOC
			Assert.IsNotNull(test.context);
		}

		[Test]
		public void TestCreationByNewFactory() {
			MiniocContext context = ContextHelper.CreateContext();

			// Then let's create a standard factory we will use
			IDIBinder binder = context.CreateBinder<SingletonBinder>();

			// Let's bind a test type using our factory
			binder.Bind<IDependencyTest>(() => new DependencyTest());

			// Let's see that the dependencies are now injected - this class has injected context
			IDependencyTest test = context.Resolve<IDependencyTest>();
			Assert.IsNotNull(test.context);
		}

		// Let's ensure a factory can work correctly with multiple / singletone
		[Test]
		public void TestCreationByNewFactorySingleton() {
			MiniocContext context = ContextHelper.CreateContext();
			BindHelper b = context.CreateBindHelper();

			// Let's bind a test type using singleton binder
			b.singleton.Bind<IDependencyTest>(() => new DependencyTest());
			
			// Let's see that the dependencies are now injected - this class has injected context
			IDependencyTest test1 = context.Resolve<IDependencyTest>();
			IDependencyTest test2 = context.Resolve<IDependencyTest>();
			Assert.AreSame(test1, test2);
		}

		[Test]
		public void TestCreationByNewFactoryMultiple() {
			MiniocContext context = ContextHelper.CreateContext();

			// Then let's create a standard factory we will use
			IDIBinder binder = context.CreateBinder<MultipleBinder>();

			// Let's bind a test type using our factory
			binder.Bind<IDependencyTest>(() => new DependencyTest());
			
			// Let's see that the dependencies are now injected - this class has injected context
			IDependencyTest test1 = context.Resolve<IDependencyTest>();
			IDependencyTest test2 = context.Resolve<IDependencyTest>();
			Assert.AreNotSame(test1, test2);
		}


		[Test]
		public void Test2InterfacesContainerBinding() {
			MiniocContext context = ContextHelper.CreateContext();
			BindHelper b = context.CreateBindHelper ();

			// Then creating bindings for 2 interfaces through the container
			b.group.BindMany<IAdvancedRead, IAdvancedWrite>(() => new AdvancedModel());

			IAdvancedRead read = context.Resolve<IAdvancedRead>();
			IAdvancedWrite write = context.Resolve<IAdvancedWrite>();
			
			Assert.That(read is AdvancedModel);
			Assert.That(write is AdvancedModel);
			
			Assert.AreSame(read, write);			
		}

		[Test]
		public void Test2InterfacesContainerBindingPersistence() {
			MiniocContext context = ContextHelper.CreateContext();
			IDIBinder modelBinder = context.CreateBinder<GroupSingletonBinder>();

			modelBinder.BindMany<IAdvancedRead, IAdvancedWrite>(() => new AdvancedModel());
			
			SingletoneCreation(context);
			IAdvancedRead read = context.Resolve<IAdvancedRead>();
			
			Assert.That(read is AdvancedModel);
			Assert.AreEqual("my_test_model", read.tag);
		}

		
		private void SingletoneCreation(MiniocContext context) {
			IAdvancedWrite write = context.Resolve<IAdvancedWrite>();
			Assert.That(write is AdvancedModel);
			write.tag = "my_test_model";
		}
      
    }
}

