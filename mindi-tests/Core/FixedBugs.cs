using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

namespace MinDI.Tests
{
    [TestFixture]
    [Category("Unit tests")]
    internal class FixedBugs
    {
		interface IApple {
		}

		class Apple : IApple {
			
		}


		[Test]
		public void TestNullNameResolve() {
			IDIContext context = ContextHelper.CreateContext();

			context.m().Bind<IApple>(() => new Apple());

			Apple appleInstance = context.Resolve<IApple, Apple>(null);
			Assert.IsNotNull(appleInstance);

			appleInstance = context.Resolve<IApple, Apple>("");
			Assert.IsNotNull(appleInstance);
		}
    }
}

