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

		public object resolve(Type type, string name, InjectionContext injectionContext) {
			object obj;
			bool result = tryResolve(type, name, injectionContext, out obj);
			if (!result) {
				return null;
			}
			return obj;
		}

		public bool tryResolve(Type type, string name, InjectionContext injectionContext, out object result) {
			TypeBindings typeBindings;
			if (!_bindings.TryGetValue(type, out typeBindings)) {
				result = null;
				return false;
			}
			result = typeBindings.resolve(name, injectionContext);
			return true;
		}

		public BindingDescriptor introspect(Type type, string name) {
			TypeBindings typeBindings;
			if (!_bindings.TryGetValue(type, out typeBindings)) {
				return null;

			}
		
			return typeBindings.introspect(name);
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
	}
}