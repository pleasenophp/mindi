using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using minioc;
using minioc.context.bindings;
using MinDI.Binders;

namespace MinDI.Tests
{

	[TestFixture]
    internal class ContextChainingTests
	{

		interface IContainer {
			IAdapter adapter {get;}
		}

		interface IAdapter {
		}

		class Container : IContainer {
			[Injection]
			public IAdapter adapter {get; set;}
		}

		class DefaultAdapter : IAdapter {
		}

		class CustomAdapter : IAdapter {
		}
			

		[Test]
		public void TestParentContainerChildAdapter() {
			MiniocContext context = new MiniocContext ();

			context.Register(Bindings.ForType<IContainer>()
				.ImplementedBy(()=>new Container())
				.SetInstantiationMode(InstantiationMode.SINGLETON));

			context.Register(Bindings.ForType<IAdapter>()
					.ImplementedBy(()=>new DefaultAdapter())
				.SetInstantiationMode(InstantiationMode.SINGLETON));


			MiniocContext childContext = new MiniocContext (context);

			childContext.Register(Bindings.ForType<IAdapter>()
				.ImplementedBy(()=>new CustomAdapter())
				.SetInstantiationMode(InstantiationMode.SINGLETON));

		
			// Resolving container from the parent context
			// Expecting DefaultAdapter
			IContainer container = context.Resolve<IContainer> ();
			Assert.IsAssignableFrom<Container> (container);
			Assert.IsAssignableFrom<DefaultAdapter> (container.adapter);

			// Resolving container from the child context
			// Expecting CustomAdapter
			container = childContext.Resolve<IContainer> ();
			Assert.IsAssignableFrom<Container> (container);
			Assert.IsAssignableFrom<CustomAdapter> (container.adapter);

		}

      
    }
}

