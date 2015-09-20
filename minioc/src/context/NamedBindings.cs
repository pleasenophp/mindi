using System.Collections.Generic;
using System.Linq;
using minioc.context.bindings;
using minioc.misc;
using minioc.resolution.core;
using MinDI.Introspection;
using MinDI;

namespace minioc.context {
	internal class NamedBindings {
		private Dictionary<string, IBinding> _namedBindings;


		/*
		internal void addBinding(BindingImpl binding) {
			createNamedBindingsIfNecessary();
			if (_namedBindings.ContainsKey(binding.name)) {
				string message;
				if (binding.name == BindingImpl.UNNAMED_BINDING) {
					message = "An unnamed Binding already exist for type " + binding.type;
				}
				else {
					message = string.Format("A Binding named '{0}' already exist for type {1}", binding.name, binding.type);
				}
				throw new MiniocException(message);
			}
			_namedBindings.Add(binding.name, binding);
			if (binding.isDefault || (binding.name == BindingImpl.UNNAMED_BINDING) || (_default == null)) {
				if ((_default == null) || !_default.isDefault) {
					_default = binding;
				}
			}
		}

		private void createNamedBindingsIfNecessary() {
			if (_namedBindings == null) {
				_namedBindings = new Dictionary<string, BindingImpl>();
			}
		}

		private object resolveDefault(InjectionContext injectionContext) {
			if (_default == null) {
				throw new MiniocException("No default Binding set for type " + _namedBindings.Values.First().type);
			}
			return _default.getInstance(injectionContext);
		}


		internal object resolve(string name, InjectionContext injectionContext) {
			if (string.IsNullOrEmpty(name)) {
				return resolveDefault(injectionContext);
			}

			BindingImpl binding;
			if ((_namedBindings == null) || !_namedBindings.TryGetValue(name, out binding)) {
				return null;
			}
			return binding.getInstance(injectionContext);
		}

		internal BindingImpl introspect(string name) {
			if (string.IsNullOrEmpty(name)) {
				if (_default == null) {
					throw new MiniocException("No default Binding set for type " + _namedBindings.Values.First().type);
				}
				return _default;
			}

			BindingImpl binding;
			if (_namedBindings == null || !_namedBindings.TryGetValue(name, out binding)) {
				return null;
			}

			return binding;
		}

		internal void removeBinding(BindingImpl bindingImpl) {
			_namedBindings.Remove(bindingImpl.name);
			if (_default == bindingImpl) {
				_default = null;
			}
		}
		*/
	}
}