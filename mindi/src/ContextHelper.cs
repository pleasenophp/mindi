using System;
using System.Collections;
using minioc;

using MinDI.Binders;
using minioc.resolution.dependencies;
using MinDI.Context;
using System.Reflection;


namespace MinDI {
	public static class ContextHelper {
		public static IDIContext CreateContext(IDIContext parent = null, string name = null) {
			return InternalCreateContext(parent, name); 
		}

		public static IDIContext CreateContext<T>(IDIContext parent = null, string name = null) where T:class, IContextInitializer {
			IDIContext context = InternalCreateContext(parent, name); 
			context.Initialize<T>();
			return context;
		}

		public static IDIContext Reproduce(this IDIContext parent, string contextName = null) {
			return CreateContext(parent, contextName);
		}

		public static IDIContext Reproduce<T>(this IDIContext parent, string contextName = null) where T:class, IContextInitializer {
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


		public static bool HasContext(this Assembly assembly) {
			object[] attributes = assembly.GetCustomAttributes(typeof(ContextAssemblyAttribute), false);	
			if (attributes.Length == 0) {
				return false;
			}
			return true;
		}

		public static bool IsUnityProjectAssembly(this Assembly assembly) {
			// NOTE - pretty lame way to do it like this, but seems to be ok for now
			return assembly.FullName.Contains("Unity") || assembly.FullName.Contains("Assembly-CSharp");
		}

	

	

		private static IDIContext InternalCreateContext(IDIContext parent, string name) {
			IDIContext context = new MiniocContext(parent, name);

			// Binding context itself
			context.s().BindInstance<IDIContext>(context);

			return context;
		}

			
	}
}
