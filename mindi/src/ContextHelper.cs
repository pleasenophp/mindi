using System;
using System.Collections;
using minioc;
using minioc.attributes;
using minioc.context.bindings;
using minioc.resolution.instantiator;
using MinDI.Binders;


namespace MinDI {
	public static class ContextHelper
	{
		public static MiniocContext CreateContext (MiniocContext parent = null, string name = null)
		{
			MiniocContext context = new MiniocContext (parent, name);
			
			// Binding context itself
			context.Register (Bindings
			                 .ForType<MiniocContext> ()
			                 .ImplementedByInstance (context).SetInstantiationMode (InstantiationMode.SINGLETON));

			return context;

		}
			

		/// <summary>
		/// Facilitates the creation of the object by automatically injecting the dependencies
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="obj">Object.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T Create<T> (MiniocContext context, T obj)
		{
			context.InjectDependencies (obj);
			return obj;
		}


		/// <summary>
		/// Creates the bind helper to simplify binding syntax.
		/// Extension of the MiniocContext.
		/// </summary>
		/// <returns>The bind helper.</returns>
		/// <param name="context">Context.</param>
		public static BindHelper CreateBindHelper(this MiniocContext context) {
			return new BindHelper (context);
		}

		/// <summary>
		/// Extension for the MiniocContext to easyly create IDIBinders
		/// </summary>
		/// <returns>The binder.</returns>
		/// <param name="context">Context.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T CreateBinder<T> (this MiniocContext context) where T:IDIBinder
		{
			return Create<T> (context, Activator.CreateInstance<T> ());
		}
		
	}
}
