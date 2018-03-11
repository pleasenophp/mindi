using System.Collections.Generic;
using minioc.misc;
using MinDI;
using System;

namespace minioc.context {
	internal class NamedBindings {
		private readonly Type type;
		private readonly IDictionary<string, IBinding> namedBindings = new Dictionary<string, IBinding>();
		private IBinding defaultBinding;

		public NamedBindings(Type type) {
			this.type = type;
		}

		public void Add(IBinding binding) {
			if (binding.makeDefault && defaultBinding != null && defaultBinding.makeDefault) {
				throw new MiniocException("Default binding already exists for type "+type);
			}

			if (namedBindings.Get(binding.name) != null) {
				throw new MiniocException(string.Format("A binding named '{0}' already exist for type {1}",
					binding.name, type));
			}

			namedBindings[binding.name] = binding;
			if (CanMakeDefault(binding)) {
				defaultBinding = binding;
			}
		}

		public void Remove(IBinding binding) {
			this.namedBindings.Remove(binding.name);
			if (this.defaultBinding == binding) {
				this.defaultBinding = null;
			}
		}
			
		public IBinding GetBinding(string name) {
			if (string.IsNullOrEmpty(name)) {
				if (defaultBinding == null) {
					throw new MiniocException("No default Binding set for type " + this.type);
				}
				return defaultBinding;
			}
			return this.namedBindings.Get(name, () => null);
		}

		private bool CanMakeDefault(IBinding binding) {
			return this.defaultBinding == null || !this.defaultBinding.makeDefault && binding.makeDefault;
		}
	}
}