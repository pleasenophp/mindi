using System;
using System.Collections.Generic;
using minioc.resolution.dependencies;
using minioc.resolution.instantiator;
using minioc.resolution.lifecycle;
using minioc.resolution.lifecycle.providers;
using minioc.misc;
using minioc.resolution.core;
using MinDI;
using MinDI.Introspection;
using MinDI.StateObjects;

namespace minioc.context.bindings {
	public class BindingImpl : IBinding, BindingStub {
		public const string UNNAMED_BINDING = "__$UNNAMED__";

		public Type type { get; private set; }
		public string name {
			get {
				return descriptor.name;
			}
		}

		public bool isDefault {
			get {
				return descriptor.isDefault;
			}
		}

		public BindingDescriptor descriptor { get; private set; }

		private BoundValueProvider _valueProvider = BoundValueProviderNotSet.INSTANCE;
		private InstantiationMode _instantiationMode = InstantiationMode.MULTIPLE;

		private BoundInstanceFactory _boundInstanceFactory;

		public BindingImpl(Type type, string name) {
			this.type = type;
			this.descriptor = new BindingDescriptor();
			this.descriptor.name = name == null?UNNAMED_BINDING:name;
		}

		public void InitFromGeneric(BindingImpl genericBinding, Func<object> factory) {
			this.descriptor.InitFromGeneric(genericBinding.descriptor, factory);
			this.ImplementedBy(factory);
			if (this.descriptor.instantiationType == InstantiationType.Concrete) {
				this.SetInstantiationMode(InstantiationMode.SINGLETON);
			}
		}

		public IBinding ImplementedByInstance(object instance, bool omitTypeCheck = false) {
			checkIsValueProviderSet();
			if (!omitTypeCheck && !type.IsInstanceOfType(instance)) {
				throw new MiniocException(string.Format("object {0} cannot be used as implementation of {1}", instance, type));
			}
			_valueProvider = new ExplicitValueProvider(instance);
			return this;
		}

		public IBinding ImplementedBy(Func<object> factory) {
			checkIsValueProviderSet();
			_valueProvider = new FactoryValueProvider(factory);
			return this;
		}

		private void checkIsValueProviderSet() {
			if (_valueProvider.isSet) {
				throw new MiniocException(string.Format("Binding for '{0}' is already set"));
			}
		}


		private IBinding SetInstantiationMode(InstantiationMode instantiationMode) {
			_instantiationMode = instantiationMode;
			return this;
		}


		public IBinding SetDescriptor(IDIContext context, InstantiationType instantiation, Func<object> factory) {
			this.descriptor.context = context;
			this.descriptor.instantiationType = instantiation;
			this.descriptor.factory = factory;
			return this;
		}

		public IBinding SetGenericDescriptor(IDIContext context, InstantiationType instantiation, InstantiationType genericInstantiation) {
			this.descriptor.context = context;
			this.descriptor.instantiationType = instantiation;
			this.descriptor.factory = null;
			this.descriptor.genericInstantiation = genericInstantiation;
			return this;
		}

		public IBinding InstanciatedBy(Instantiator instantiator) {
			_valueProvider.setInstantiator(instantiator);
			return this;
		}

		public IBinding MakeDefault() {
			descriptor.isDefault = true;
			return this;
		}

		internal object getInstance(InjectionContext injectionContext) {
			// NOTE - we use ST now for generics only. For everything else it's multiple mode
			// Might be reworked completely soon
			if (_boundInstanceFactory == null) {
				if (_instantiationMode == InstantiationMode.SINGLETON) {
					_boundInstanceFactory = new SingletonFactory(_valueProvider);
				}
				else {
					_boundInstanceFactory = new MultipleInstanceFactory(_valueProvider);
				}
			}

			object instance = _boundInstanceFactory.getInstance(injectionContext);

			IDIClosedContext mindiInstance = instance as IDIClosedContext;
			if (mindiInstance != null) {
				mindiInstance.descriptor.bindingDescriptor = this.descriptor;
			}

			return instance;
		}

		internal void verifyIntegrity() {
			if (!_valueProvider.isSet) {
				throw new MiniocException("No implementation has been specified for type " + type);
			}
		}
	}
}