using System.Collections.Generic;
using minioc.resolution.dependencies;
using minioc.resolution.instantiator;
using minioc.misc;
using minioc.resolution.core;
using MinDI;

namespace minioc.resolution.lifecycle.providers {
internal class ExplicitValueProvider : BoundValueProvider {
    private readonly object _instance;

    public ExplicitValueProvider(object instance) {
        _instance = instance;
    }

    public bool isSet { get { return true; } }

    public void setInstantiator(Instantiator instantiator) {
        throw new MiniocException("Instantiator cannot be set on explicit value");
    }

    public object createInstance(InjectionContext injectionContext, List<IDependency> dependencies) {
        return _instance;
    }
}
}