using System;
using System.Collections;
using minioc;
using minioc.context.bindings;
using minioc.resolution.instantiator;
using MinDI.Binders;
using minioc.resolution.dependencies;
using MinDI.Context;


namespace MinDI {
	public static class ContextHelper {
		public static IDIContext CreateContext(IDIContext parent = null, string name = null) {
			return InternalCreateContext(parent, name); 
		}

		public static IDIContext CreateContext<T>(IDIContext parent = null, string name = null) where T:IContextInitializer {
			IDIContext context = InternalCreateContext(parent, name); 
			context.Initialize<T>();
			return context;
		}
			
		/// <summary>
		/// Resolve an interface from the specified context with casting it to the instance.
		/// Can be usefull for the builders and factories
		/// </summary>
		/// <param name="context">Context.</param>
		/// <typeparam name="TInterface">The 1st type parameter.</typeparam>
		/// <typeparam name="TInstance">The 2nd type parameter.</typeparam>
		public static TInstance Resolve<TInterface, TInstance>(this IDIContext context, string name = null) 
			where TInterface : class
			where TInstance : class, TInterface 
		{
			return context.Resolve<TInterface>(name) as TInstance;
		}

		public static IDIContext Reproduce(this IDIContext parent, string contextName = null) {
			return CreateContext(parent, contextName);
		}

		public static IDIContext Reproduce<T>(this IDIContext parent, string contextName = null) where T:IContextInitializer {
			return CreateContext<T>(parent, contextName);
		}

		/*
		/// <summary>
		/// Chains an interface on the new context, rebinding it as a singletone
		/// This is usefull to build complex hierarchy of the objects
		/// Returns the new context. 
		/// </summary>
		/// <param name="context">Context.</param>
		/// <typeparam name="T">The interface type parameter.</typeparam>
		public static IDIContext Chain<T>(this IDIContext context) where T:class {
			IDIContext newContext = CreateContext(context);
			newContext.s().Rebind<T>();
			return newContext;
		}
		*/

		private static IDIContext InternalCreateContext(IDIContext parent, string name) {
			IDIContext context = new MiniocContext(parent, name);

			// Binding context itself
			context.s().BindInstance<IDIContext>(context);

			return context;
		}
			
	}
}
