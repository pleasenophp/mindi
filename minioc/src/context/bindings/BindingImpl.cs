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
		// private InstantiationMode _instantiationMode = InstantiationMode.SINGLETON;

		private BoundInstanceFactory _boundInstanceFactory;

		private List<IDependency> _dependencies = new List<IDependency>();


		public BindingImpl(Type type, string name) {
			this.type = type;
			this.descriptor = new BindingDescriptor();
			this.descriptor.name = name == null?UNNAMED_BINDING:name;
		}

		public void InitFromGeneric(BindingImpl genericBinding, Func<object> factory) {
			this._dependencies = genericBinding._dependencies;
			this.ImplementedBy(factory);
			this.descriptor.InitFromGeneric(genericBinding.descriptor, factory);
		}

		public IBinding ImplementedBy<T>() {
			checkIsValueProviderSet();
			_valueProvider = new TypeInstanceBoundValueProvider(typeof(T));
			return this;
		}

		public IBinding ImplementedBy(Type instanceType) {
			checkIsValueProviderSet();
			if (!type.IsAssignableFrom(instanceType)) {
				throw new MiniocException(string.Format("Type {0} cannot be used as implementation of {1}", instanceType, type));
			}
			_valueProvider = new TypeInstanceBoundValueProvider(instanceType);
			return this;
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

		/*
		public IBinding SetInstantiationMode(InstantiationMode instantiationMode) {
			_instantiationMode = instantiationMode;
			return this;
		}
		*/

		public IBinding SetDescriptor(IDIContext context, InstantiationType instantiation, BindingType type, Func<object> factory) {
			this.descriptor.context = context;
			this.descriptor.instantiationType = instantiation;
			this.descriptor.bindingType = type;
			this.descriptor.factory = factory;
			return this;
		}

		public IBinding InstanciatedBy(Instantiator instantiator) {
			_valueProvider.setInstantiator(instantiator);
			return this;
		}

		public IBinding DependsOn(IDependency dependency) {
			_dependencies.Add(dependency);
			return this;
		}

		public IBinding MakeDefault() {
			descriptor.isDefault = true;
			return this;
		}

		internal object getInstance(InjectionContext injectionContext) {
			// NOTE - as we use outside binders, can instantiate in always multiple mode here
			if (_boundInstanceFactory == null) {
				_boundInstanceFactory = new MultipleInstanceFactory(_valueProvider);
				/*
				if (_instantiationMode == InstantiationMode.SINGLETON) {
					_boundInstanceFactory = new SingletonFactory(_valueProvider);
				}
				else {
					_boundInstanceFactory = new MultipleInstanceFactory(_valueProvider);
				}
				*/
			}

			object instance = _boundInstanceFactory.getInstance(_dependencies, injectionContext);

			IDIClosedContext mindiInstance = instance as IDIClosedContext;
			if (mindiInstance != null) {
				mindiInstance.bindingDescriptor = this.descriptor;
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