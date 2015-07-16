using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using minioc;
using minioc.context.bindings;
using minioc.resolution.instantiator;
using System.Reflection;

namespace MinDI.Context {
	/// <summary>
	/// Context builder.
	/// This class is responsible for finding all the populators and call them to populate context
	/// NOTE - for dynamical assembly loading / unloading it will need some extra code
	/// NOTE - also will need some code if we want to easily override library context initializers (see if we really want this)
	/// </summary>
	public static class ContextBuilder {
		private static IDictionary<Type, List<IContextInitializer>> initializers = null;
		
		public static void Initialize<T>(IDIContext context) where T:IContextInitializer {	
			if (initializers == null) {
				FetchInitializers();
			}

			List<IContextInitializer> instances;
			if (!initializers.TryGetValue(typeof(T), out instances)) {
				return;
			}

			foreach (IContextInitializer instance in instances) {
				instance.Initialize(context);
			}
		}


		public static void InitSingle<T>(IDIContext context) where T:IContextInitializer {
			IContextInitializer initializer = Activator.CreateInstance<T>();
			if (initializer == null) {
				throw new MindiException("Couldn't create an initializer instance for type "+typeof(T).FullName);
			}

			initializer.Initialize(context);
		}

		private static void FetchInitializers() {
			initializers = new Dictionary<Type, List<IContextInitializer>>();

			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
				if (Assembly.GetExecutingAssembly() != assembly && Assembly.GetCallingAssembly() != assembly && 
					!assembly.IsUnityProjectAssembly() && !assembly.HasContext()) {
					continue;
				}

				foreach (Type type in assembly.GetTypes()) {
					if (type.IsClass && typeof(IContextInitializer).IsAssignableFrom(type)) {
						AddInitializer(type);
					}
				}
			}
		}

		private static void AddInitializer(Type type) {
			Type[] interfaces = type.FindInterfaces((t, c) => {
				if (t == typeof(IContextInitializer)) {
					return false;
				}

				if (!typeof(IContextInitializer).IsAssignableFrom(t)) {
					return false;
				}

				return true;

			}, null);

			if (interfaces.Length == 0) {
				throw new MindiException("Couldn't find any interface that is derived from IContextInitializer for the type "+type.FullName);
			}

			foreach (Type interfaceType in interfaces) {
				AddInitializer(interfaceType, type);
			}
		}

		private static void AddInitializer(Type interfaceType, Type type) {
			List<IContextInitializer> instances;
			if (!initializers.TryGetValue(interfaceType, out instances)) {
				instances = new List<IContextInitializer>();
			}
			initializers[interfaceType] = instances;
			instances.Add(Activator.CreateInstance(type) as IContextInitializer);
		}

		private static bool HasContext(this Assembly assembly) {
			object[] attributes = assembly.GetCustomAttributes(typeof(ContextAssemblyAttribute), false);	
			if (attributes.Length == 0) {
				return false;
			}
			return true;
		}

		private static bool IsUnityProjectAssembly(this Assembly assembly) {
			// NOTE - pretty lame way to do it like this, but seems to be ok for now
			return assembly.FullName.Contains("Unity") || assembly.FullName.Contains("Assembly-CSharp");
		}
	}
}