using System;
using System.Collections.Generic;
using minioc.resolution.dependencies;
using minioc.resolution.instantiator;
using minioc.resolution.core;

namespace minioc.resolution.lifecycle.providers {
internal class TypeInstanceBoundValueProvider : BoundValueProvider {
    private Type _type;
    private Instantiator _instantiator = ConstructorInstantiator.INSTANCE;

    public TypeInstanceBoundValueProvider(Type type) {
        _type = type;
    }

    public bool isSet { get {return true;} }

    public void setInstantiator(Instantiator instantiator) {
        _instantiator = instantiator;
    }

    public object createInstance(InjectionContext injectionContext, List<Dependency> dependencies) {
        return injectionContext.createInstance(_type, _instantiator, dependencies);
    }
}
}