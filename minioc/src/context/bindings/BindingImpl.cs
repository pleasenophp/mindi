using System;
using System.Collections.Generic;
using minioc.resolution.dependencies;
using minioc.resolution.instantiator;
using minioc.resolution.lifecycle;
using minioc.resolution.lifecycle.providers;
using minioc.misc;
using minioc.resolution.core;

namespace minioc.context.bindings {
internal class BindingImpl : Binding, BindingStub {
    public const string UNNAMED_BINDING = "__$UNNAMED__";

    public Type type { get; private set; }
    public string name { get; private set; }
    public bool isDefault { get; private set; }

    private BoundValueProvider _valueProvider = BoundValueProviderNotSet.INSTANCE;
    private InstantiationMode _instantiationMode = InstantiationMode.SINGLETON;

    private BoundInstanceFactory _boundInstanceFactory;

    private List<Dependency> _dependencies = new List<Dependency>();


    public BindingImpl(Type type, string name) {
        this.type = type;
        this.name = name == null ? UNNAMED_BINDING : name;
    }

    public Binding ImplementedBy<T>() {
        checkIsValueProviderSet();
        _valueProvider = new TypeInstanceBoundValueProvider(typeof(T));
        return this;
    }

    public Binding ImplementedBy(Type instanceType) {
        checkIsValueProviderSet();
        if (!type.IsAssignableFrom(instanceType)) {
            throw new MiniocException(string.Format("Type {0} cannot be used as implementation of {1}", instanceType, type));
        }
        _valueProvider = new TypeInstanceBoundValueProvider(instanceType);
        return this;
    }

    public Binding ImplementedByInstance(object instance) {
        checkIsValueProviderSet();
        if (!type.IsInstanceOfType(instance)) {
            throw new MiniocException(string.Format("object {0} cannot be used as implementation of {1}", instance, type));
        }
        _valueProvider = new ExplicitValueProvider(instance);
        return this;
    }

    public Binding ImplementedBy(Func<object> factory) {
        checkIsValueProviderSet();
        _valueProvider = new FactoryValueProvider(factory);
        return this;
    }

    private void checkIsValueProviderSet() {
        if (_valueProvider.isSet) {
            throw new MiniocException(string.Format("Binding for '{0}' is already set"));
        }
    }

    public Binding SetInstantiationMode(InstantiationMode instantiationMode) {
        _instantiationMode = instantiationMode;
        return this;
    }

    public Binding InstanciatedBy(Instantiator instantiator) {
        _valueProvider.setInstantiator(instantiator);
        return this;
    }

    public Binding DependsOn(Dependency dependency) {
        _dependencies.Add(dependency);
        return this;
    }

    public Binding MakeDefault() {
        isDefault = true;
        return this;
    }

    internal object getInstance(InjectionContext injectionContext) {
        if (_boundInstanceFactory == null) {
            if (_instantiationMode == InstantiationMode.SINGLETON) {
                _boundInstanceFactory = new SingletonFactory(_valueProvider);
            } else {
                _boundInstanceFactory = new MultipleInstanceFactory(_valueProvider);
            }
        }

		object instance = _boundInstanceFactory.getInstance(_dependencies, injectionContext);
		return instance;
    }

    internal void verifyIntegrity() {
        if (!_valueProvider.isSet) {
            throw new MiniocException("No implementation has been specified for type " + type);
        }
    }
}
}