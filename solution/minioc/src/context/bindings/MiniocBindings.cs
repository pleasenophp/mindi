using System;
using System.Collections.Generic;
using minioc.misc;
using minioc.resolution.core;
using MinDI;
using MinDI.Introspection;

namespace minioc.context {
	internal class MiniocBindings {
		private readonly Dictionary<Type, NamedBindings> bindings = new Dictionary<Type, NamedBindings>();

		public void Add(IBinding binding) {
			foreach (Type type in binding.types) {
				AddBindingForType(type, binding);
			}
		}

		private void AddBindingForType(Type type, IBinding binding) {
			NamedBindings descriptor = bindings.Get(type, () => new NamedBindings(type));
			bindings[type] = descriptor;
			descriptor.Add(binding);
		}

		public void Remove(IBinding binding) {
			foreach (Type type in binding.types) {
				RemoveBindingForType(type, binding);
			}
		}

		private void RemoveBindingForType(Type type, IBinding binding) {
			NamedBindings descriptor = bindings.Get(type, () => null);
			if (descriptor != null) {
				descriptor.Remove(binding);
			}
		}

		public object Resolve(Type type, string name) {
			IBinding binding = Introspect(type, name);
			if (binding != null) {
				return binding.Resolve();
			}
			return null;
		}
			
		public IBinding Introspect(Type type, string name) {
			IBinding binding = GetBinding (type, name);
			if (binding != null) {
				return binding;
			}

			// Checking for generic
			if (type.IsGenericType) {
				return TryIntrospectGeneric (type, name);
			}

			return null;
		}

		private IBinding GetBinding (Type type, string name)
		{
			NamedBindings descriptor = bindings.Get (type);
			if (descriptor == null) {
				return null;
			}

			IBinding binding = descriptor.GetBinding (name);
			if (binding != null) {
				return binding;
			}
			return null;
		}

		private IBinding TryIntrospectGeneric(Type type, string name) {
			if (!TryCreateBindingFromGeneric(type, name)) {
				return null;
			}
			return Introspect(type, name);
		}

		private bool TryCreateBindingFromGeneric(Type type, string name) {
			Type definition = type.GetGenericTypeDefinition();
			NamedBindings descriptor = this.bindings.Get(definition);
			if (descriptor == null) {
				return false;
			}
				
			IBinding genericBinding = descriptor.GetBinding(name);
			if (genericBinding == null) {
				return false;
			}

			IBinding concreteBinding = Binding.CreateFromGeneric(genericBinding, type.GetGenericArguments());
			this.Add(concreteBinding);
			return true;
		}

	}
}