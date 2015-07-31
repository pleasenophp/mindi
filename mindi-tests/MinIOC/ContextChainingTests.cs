using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using minioc;
using minioc.context.bindings;
using MinDI.Binders;

namespace MinDI.Tests.MinIOC
{

	[TestFixture]
    internal class ContextChainingTests
	{

		interface IContainer {
			IAdapter adapter {get;}
		}

		interface IAdapter {
		}

		class Container : ContextObject, IContainer {
			[Injection]
			public IAdapter adapter {get; set;}
		}

		class DefaultAdapter : ContextObject, IAdapter {
		}

		class CustomAdapter : ContextObject, IAdapter {
		}
			

		[Test]
		public void TestParentContainerChildAdapter() {
			IDIContext context = ContextHelper.CreateContext();

			context.m().Bind<IContainer>(() => new Container());
			context.m().Bind<IAdapter>(() => new DefaultAdapter());

			IDIContext childContext = ContextHelper.CreateContext(context);
			childContext.m().Bind<IAdapter>(() => new CustomAdapter());
		
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

