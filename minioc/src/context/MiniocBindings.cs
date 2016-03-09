using System;
using System.Collections.Generic;
using minioc.context.bindings;
using minioc.misc;
using minioc.resolution.core;
using MinDI;
using MinDI.Introspection;

namespace minioc.context {
	internal class MiniocBindings {
		private Dictionary<Type, TypeBindings> _bindings = new Dictionary<Type, TypeBindings>();

		public bool tryGetValue(Type type, out TypeBindings typeBindings) {
			return _bindings.TryGetValue(type, out typeBindings);
		}

		public void add(BindingImpl impl) {
			TypeBindings typeBindings;
			if (!_bindings.TryGetValue(impl.type, out typeBindings)) {
				typeBindings = new TypeBindings();
				_bindings.Add(impl.type, typeBindings);
			}
			typeBindings.addBinding(impl);
		}

		public bool tryResolve(Type type, string name, InjectionContext injectionContext, out object result) {
			if (type.IsGenericTypeDefinition) {
				throw new MiniocException("Generic type definitions may not be resolved ! Passed type: "+type);
			}

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
			if (result == null) {
				return false;
			}

			return true;
		}

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
		}

		public void remove(IBinding binding) {
			BindingImpl impl = binding as BindingImpl;
			if (impl == null) {
				return;
			}
			TypeBindings typeBindings;
			if (!_bindings.TryGetValue(impl.type, out typeBindings)) {
				return;
			}
			typeBindings.removeBinding(impl);
		}

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
	}
}