using System;
using System.Collections.Generic;
using System.Linq;
using MinDI.Context;

namespace MinDI {
	/// <summary>
	/// Context builder.
	/// This class is responsible for finding all the populators and call them to populate context
	/// NOTE - for dynamical assembly loading / unloading it will need some extra code
	/// NOTE - also will need some code if we want to easily override library context initializers (see if we really want this)
	/// </summary>
	public static class ContextBuilder {
	    private static IDictionary<Type, List<IContextInitializer>> initializers = null;

		private static IContextBuilderTypesProvider typesProvider;

	    /// <summary>
	    /// Set it to override the types provider with a custom one
	    /// </summary>
		public static IContextBuilderTypesProvider TypesProvider {
		    get
		    {
		        return typesProvider ?? (typesProvider = new ReflectionTypesProvider());
		    }
			set {
				typesProvider = value;
			}
		}

	    /// <summary>
	    /// If set, the types provider will try to preload assemblies from files.
	    /// Don't set it for mobile platforms and WebGL, where there is no access to file system
	    /// </summary>
	    public static bool PreloadAssemblies { get; set; }

	    static ContextBuilder()
	    {
	        PreloadAssemblies = true;
	    }

	    public static IList<T> Initialize<T>(this IDIContext context, FilteredInitializerAttribute filter = null) where T:class, IContextInitializer {
			if (!typeof(T).IsInterface) {
				return new List<T>{InitSingle<T>(context)};
			}

			if (typeof(T) == typeof(ICustomContextInitializer)) {
				throw new MindiException(string.Format(
					"{0} can only be used for single-class context initializers !", 
					typeof(ICustomContextInitializer)
				));
			}

			if (initializers == null) {
				FetchInitializers();
			}

			IList<T> result = new List<T>();

			List<IContextInitializer> instances;
			if (!initializers.TryGetValue(typeof(T), out instances)) {
				return result;
			}

			foreach (var instance in instances) {
				if (filter != null && IsInstanceFiltered(instance, filter)) {
					continue;
				}

				instance.Initialize(context);
				result.Add((T)instance);
			}
			return result;
		}


		private static T InitSingle<T>(IDIContext context) where T:class, IContextInitializer {
			var initializer = Activator.CreateInstance<T>() as T;
			if (initializer == null) {
				throw new MindiException("Couldn't create an initializer instance for type "+typeof(T).FullName);
			}

			initializer.Initialize(context);
			return initializer;
		}

		private static bool IsInstanceFiltered(IContextInitializer instance, FilteredInitializerAttribute filter) {
			var attributes = instance.GetType().GetCustomAttributes(filter.GetType(), true);
			return attributes.Cast<FilteredInitializerAttribute>().All(atr => atr.name != filter.name);
		}

		private static void FetchInitializers() {
			initializers = new Dictionary<Type, List<IContextInitializer>>();

			foreach (Type type in TypesProvider.GetTypes()) {
				AddInitializer(type);
			}
		}

		private static void AddInitializer(Type type) {
			var interfaces = type.FindInterfaces((t, c) => t != typeof(IContextInitializer) && typeof(IContextInitializer).IsAssignableFrom(t), null);
			if (interfaces.Length == 0) {
				throw new MindiException("Couldn't find any interface that is derived from IContextInitializer for the type "+type.FullName);
			}
			foreach (var interfaceType in interfaces) {
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

	}
}