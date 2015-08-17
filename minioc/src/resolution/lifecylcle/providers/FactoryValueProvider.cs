using System;
using System.Collections.Generic;
using minioc.resolution.dependencies;
using minioc.resolution.instantiator;
using minioc.misc;
using minioc.resolution.core;
using MinDI;

namespace minioc.resolution.lifecycle.providers {
internal class FactoryValueProvider : BoundValueProvider {
    private readonly Func<object> _factory;

    public FactoryValueProvider(Func<object> factory) {
        _factory = factory;
    }

    public bool isSet { get { return true; } }

    public void setInstantiator(Instantiator instantiator) {
        throw new MiniocException("Instantiator cannot be set on factory value");
    }

    public object createInstance(InjectionContext injectionContext) {
        return _factory();
    }
}
}