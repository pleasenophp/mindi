using System;
using System.Collections;
using minioc;
using minioc.context.bindings;
using MinDI.Binders;
using minioc.resolution.dependencies;
using MinDI.Context;


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

		private static IDIContext InternalCreateContext(IDIContext parent, string name) {
			IDIContext context = new MiniocContext(parent, name);

			// Binding context itself
			context.s().BindInstance<IDIContext>(context);

			return context;
		}
			
	}
}
