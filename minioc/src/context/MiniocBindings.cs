using System;
using System.Collections.Generic;
using minioc.context.bindings;
using minioc.misc;
using minioc.resolution.core;
using MinDI;
using MinDI.Introspection;

namespace minioc.context {
	internal class MiniocBindings {
		private Dictionary<Type, NamedBindings> bindings = new Dictionary<Type, NamedBindings>();

		public void Add(IBinding binding) {
			foreach (Type type in binding.types) {
				AddBindingForType(type, binding);
			}
		}

		private void AddBindingForType(Type type, IBinding binding) {
			NamedBindings descriptor = bindings.Get(type, () => new NamedBindings());
			bindings[type] = descriptor;
			descriptor.Add(type, binding);
		}

		public void Remove(IBinding binding) {
			foreach (Type type in binding.types) {
				RemoveBindingForType(type, binding);
			}
		}

		private void RemoveBindingForType(Type type, IBinding binding) {
			NamedBindings descriptor = bindings.Get(type, () => null);
			if (descriptor != null) {
				descriptor.Remove(type, binding);
			}
		}

		public bool TryResolve(Type type, string name, out object result) {
			result = null;
			if (type.IsGenericTypeDefinition) {
				throw new MiniocException("Generic type definitions may not be resolved ! Passed type: "+type);
			}

			NamedBindings descriptor = bindings.Get(type);
			if (descriptor == null) {
				return false;
			}

			return descriptor.TryResolve(type, name, out result);


			/*
			TypeBindings typeBindings;
			if (!_bindings.TryGetValue(type, out typeBindings)) {

				// Resolving generic type from the definition binding, if the type binding is not found
				if (type.IsGenericType) {
					return tryResolveGeneric(type, name, injectionContext, out result);
				}

				result = null;
				return false;
			}


			result = typeBindings.resolve(name, injectionContext);
			return true;
			*/
		}

		// TODO - restore
		/*
		public BindingDescriptor introspect(Type type, string name) {
			TypeBindings typeBindings;
			if (!_bindings.TryGetValue(type, out typeBindings)) {
				// Resolving generic type from the definition binding, if the type binding is not found
				if (type.IsGenericType) {
					return tryIntrospectGeneric(type, name);
				}

				return null;

			}

			BindingImpl binding = typeBindings.introspect(name);
			if (binding == null) {
				return null;
			}

			return binding.descriptor;
		}*/


		/*
		private bool tryInstantiateGeneric(Type type, string name) {
			Type definition = type.GetGenericTypeDefinition();
			TypeBindings typeBindings;
			if (!_bindings.TryGetValue(definition, out typeBindings)) {
				return false;
			}

			GenericTypeResolver resolver = typeBindings.resolve(name, null) as GenericTypeResolver;
			if (resolver == null) {
				return false;
			}

			BindingImpl genericBinding = typeBindings.introspect(name);
			BindingImpl binding = resolver.CreateBinding(type, genericBinding);
			binding.verifyIntegrity();
			this.add(binding);
			return true;
		}

		private BindingDescriptor tryIntrospectGeneric(Type type, string name) {
			if (!tryInstantiateGeneric(type, name)) {
				return null;
			}

			return introspect(type, name);
		}

		private bool tryResolveGeneric(Type type, string name, InjectionContext injectionContext, out object result) {
			if (!tryInstantiateGeneric(type, name)) {
				result = null;
				return false;
			}
			return tryResolve(type, name, injectionContext, out result);
		}
		*/
	}
}