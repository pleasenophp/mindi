using minioc;
using MinDI.Context;
using System.Reflection;


namespace MinDI {
	public static class ContextHelper {
		public static IDIContext CreateContext(IDIContext parent = null, string name = null) {
			return InternalCreateContext(parent, name); 
		}

		public static IDIContext CreateContext<T>(IDIContext parent = null, string name = null) where T:class, IContextInitializer {
			var context = InternalCreateContext(parent, name); 
			context.Initialize<T>();
			return context;
		}

		public static IDIContext Reproduce(this IDIContext parent, string contextName = null) {
			return CreateContext(parent, contextName);
		}

		public static IDIContext Reproduce<T>(this IDIContext parent, string contextName = null) where T:class, IContextInitializer {
			return CreateContext<T>(parent, contextName);
		}


		public static bool HasContext(this Assembly assembly) {
			var attributes = assembly.GetCustomAttributes(typeof(ContextAssemblyAttribute), false);	
			return attributes.Length != 0;
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
