using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using MinDI.Introspection;
using MinDI.StateObjects;

namespace MinDI.Tests
{
    [TestFixture]
    [Category("Unit tests")]
    internal class Regression {

		private interface IMyClass {}

		private class MyClass : IMyClass {
		}

		interface IApple {
		}

		interface ISeed {
		}

		interface ISkin {
		}

		class Apple : ContextObject, IApple {

			[SoftRequirement] public ISkin skin { get; set;}
			[SoftRequirement] public ISeed seed { get; set;}

		}

		class BigApple : IApple {
		}

		class Seed : ISeed {
			
		}

		class Skin: ISkin {
		}


		interface IPineapple
		{
			IOrange orange { get; }
		}

		interface IOrange
		{
		}

		class Pineapple : ContextObject, IPineapple
		{
			[Injection]
			public IOrange orange { get; set; }
		}

		class Orange1 : ContextObject, IOrange
		{
		}

		class Orange2 : ContextObject, IOrange
		{
		}


		[Test]
		public void TestNullNameResolve ()
		{
			IDIContext context = ContextHelper.CreateContext ();

			context.m ().Bind<IPineapple> (() => new Pineapple ());
			context.m ().Bind<IOrange> (() => new Orange1 ());

			IPineapple appleInstance = context.Resolve<IPineapple> (null, null);
			Assert.IsNotNull (appleInstance);

			appleInstance = context.Resolve<IPineapple> ("");
			Assert.IsNotNull (appleInstance);
		}

		[Test]
		public void TestResolvedInstanceHasBindingDescriptor ()
		{
			IDIContext context = ContextHelper.CreateContext ();
			IOrange orange = new Orange1();
			(orange as IDIClosedContext).Invalidate();
			context.s().BindInstance(orange);
			context.m().Bind<IPineapple>(() => new Pineapple());

			IPineapple pineApple = context.Resolve<IPineapple>();

			Assert.IsNotNull(pineApple);
			Assert.IsNotNull(pineApple.orange);
		}


		[Test]
		public void TestResolveObjectWithInvalidatedContext ()
		{
			IDIContext context = ContextHelper.CreateContext ();
			context.m ().Bind<IOrange> (() => new Orange1 ());

			IOrange orange = context.Resolve<IOrange> ();
			IDIClosedContext ctx = orange as IDIClosedContext;

			Assert.IsNotNull (ctx.descriptor.bindingDescriptor);
		}

		[Test]
		public void TestSTNotSubjective ()
		{
			IDIContext context = ContextHelper.CreateContext ();
			context.m ().Bind<IOrange> (() => new Orange1 ());
			context.s ().Bind<IPineapple> (() => new Pineapple ());
			IDIContext childContext = context.Reproduce ();
			childContext.m ().Bind<IOrange> (() => new Orange2 ());

			IPineapple apple2 = childContext.Resolve<IPineapple> ();
			IPineapple apple1 = context.Resolve<IPineapple> ();

			Assert.AreSame (apple1, apple2);
			Assert.IsInstanceOf (typeof (Orange1), apple1.orange);
			Assert.IsInstanceOf (typeof (Orange1), apple2.orange);
		}

		[Test]
		public void TestMPSubjective ()
		{
			IDIContext context = ContextHelper.CreateContext ();
			context.m ().Bind<IOrange> (() => new Orange1 ());
			context.m ().Bind<IPineapple> (() => new Pineapple ());
			IDIContext childContext = context.Reproduce ();
			childContext.m ().Bind<IOrange> (() => new Orange2 ());

			IPineapple apple2 = childContext.Resolve<IPineapple> ();
			IPineapple apple1 = context.Resolve<IPineapple> ();

			Assert.AreNotSame (apple1, apple2);
			Assert.IsInstanceOf (typeof (Orange1), apple1.orange);
			Assert.IsInstanceOf (typeof (Orange2), apple2.orange);
		}

		[Test]
		public void TestNamedParentResolution() {
			IDIContext parentContext = ContextHelper.CreateContext();
			parentContext.m().Bind<IApple>(() => new Apple(), BindingName.ForType<Apple>());

			IDIContext context = parentContext.Reproduce();
			context.m().Bind<IApple>(() => new BigApple(), BindingName.ForType<BigApple>());

			IApple apple = context.Resolve<IApple>(BindingName.ForType<BigApple>());
			Assert.That(apple is BigApple);

			apple = context.Resolve<IApple>();
			Assert.That(apple is BigApple);

			apple = context.Resolve<IApple>(BindingName.ForType<Apple>());
			Assert.That(apple is Apple);
		}


		[Test]
		public void TestNamedParentIntrospection() {
			IDIContext parentContext = ContextHelper.CreateContext();
			parentContext.m().Bind<IApple>(() => new Apple(), BindingName.ForType<Apple>());

			IDIContext context = parentContext.Reproduce();
			context.m().Bind<IApple>(() => new BigApple(), BindingName.ForType<BigApple>());

			IBinding desc = context.Introspect<IApple>(BindingName.ForType<BigApple>());
			Assert.IsNotNull(desc);
			Assert.AreEqual(InstantiationType.Abstract, desc.instantiationType);
			Assert.That(desc.context == context);
			object apple = desc.factory();
			Assert.That(apple is BigApple);

			desc = context.Introspect<IApple>();
			Assert.IsNotNull(desc);
			Assert.AreEqual(InstantiationType.Abstract, desc.instantiationType);
			Assert.That(desc.context == context);
			apple = desc.factory();
			Assert.That(apple is BigApple);

			desc = context.Introspect<IApple>(BindingName.ForType<Apple>());
			Assert.IsNotNull(desc);
			Assert.AreEqual(InstantiationType.Abstract, desc.instantiationType);
			Assert.That(desc.context == parentContext);
			apple = desc.factory();
			Assert.That(apple is Apple);
		}

		[Test]
		public void TestNullBinding() {
			IDIContext context = ContextHelper.CreateContext();
			context.s().BindInstance<IApple>(null);

			IApple apple = context.TryResolve<IApple>();
			Assert.IsNull(apple);
		}

    }
}

