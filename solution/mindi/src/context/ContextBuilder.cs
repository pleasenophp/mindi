﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using minioc;

using System.Reflection;
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
		public static IContextBuilderTypesProvider TypesProvider {
			get {
				if (typesProvider == null) {
					typesProvider = new ReflectionTypesProvider();
				}

				return typesProvider;
			}
			set {
				typesProvider = value;
			}
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

			foreach (IContextInitializer instance in instances) {
				if (filter != null && IsInstanceFiltered(instance, filter)) {
					continue;
				}

				instance.Initialize(context);
				result.Add((T)instance);
			}
			return result;
		}


		private static T InitSingle<T>(IDIContext context) where T:class, IContextInitializer {
			T initializer = Activator.CreateInstance<T>() as T;
			if (initializer == null) {
				throw new MindiException("Couldn't create an initializer instance for type "+typeof(T).FullName);
			}

			initializer.Initialize(context);
			return initializer;
		}

		private static bool IsInstanceFiltered(IContextInitializer instance, FilteredInitializerAttribute filter) {
			object[] attributes = instance.GetType().GetCustomAttributes(filter.GetType(), true);
			foreach (FilteredInitializerAttribute atr in attributes) {
				if (atr.name == filter.name) {
					return false;
				}
			}
			return true;
		}

		private static void FetchInitializers() {
			initializers = new Dictionary<Type, List<IContextInitializer>>();

			foreach (Type type in TypesProvider.GetTypes()) {
				AddInitializer(type);
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

	}
}