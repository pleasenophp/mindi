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
		interface IApple
		{
			IOrange orange { get; }
		}

		interface IOrange
		{
		}

		class Apple : ContextObject, IApple
		{
			[Injection] public IOrange orange {get; set; }
		}

		class Orange1 : IOrange
		{
		}

		class Orange2 : IOrange
		{
		}


		[Test]
		public void TestNullNameResolve() {
			IDIContext context = ContextHelper.CreateContext();

			context.m().Bind<IApple>(() => new Apple());
			context.m().Bind<IOrange> (() => new Orange1 ());

			IApple appleInstance = context.Resolve<IApple>(null, null);
			Assert.IsNotNull(appleInstance);

			appleInstance = context.Resolve<IApple>("");
			Assert.IsNotNull(appleInstance);
		}

		[Test]
		public void TestSTNotSubjective ()
		{
			IDIContext context = ContextHelper.CreateContext ();
			context.m ().Bind<IOrange> (() => new Orange1 ());
			context.s ().Bind<IApple> (() => new Apple ());
			IDIContext childContext = context.Reproduce ();
			childContext.m ().Bind<IOrange> (() => new Orange2 ());

			IApple apple2 = childContext.Resolve<IApple> ();
			IApple apple1 = context.Resolve<IApple> ();

			Assert.AreSame (apple1, apple2);
			Assert.IsInstanceOf (typeof (Orange1), apple1.orange);
			Assert.IsInstanceOf (typeof (Orange1), apple2.orange);
		}

		[Test]
		public void TestMPSubjective ()
		{
			IDIContext context = ContextHelper.CreateContext ();
			context.m ().Bind<IOrange> (() => new Orange1 ());
			context.m ().Bind<IApple> (() => new Apple ());
			IDIContext childContext = context.Reproduce ();
			childContext.m ().Bind<IOrange> (() => new Orange2 ());

			IApple apple2 = childContext.Resolve<IApple> ();
			IApple apple1 = context.Resolve<IApple> ();

			Assert.AreNotSame (apple1, apple2);
			Assert.IsInstanceOf (typeof (Orange1), apple1.orange);
			Assert.IsInstanceOf (typeof (Orange2), apple2.orange);
		}
    }
}

