using System;
using System.Collections.Generic;
using minioc.resolution.dependencies;
using minioc.resolution.instantiator;
using minioc.misc;
using minioc.resolution.core;

namespace minioc.resolution.lifecycle.providers {
internal class BoundValueProviderNotSet : BoundValueProvider {
    public static readonly BoundValueProviderNotSet INSTANCE = new BoundValueProviderNotSet();
    public bool isSet { get { return false; } }
    public void setInstantiator(Instantiator instantiator) {
        throw new MiniocException("Cannot set Instantiator when no implementation is set");
    }

    public object createInstance(InjectionContext injectionContext, List<Dependency> dependencies) {
        throw new NotImplementedException();
    }

    private BoundValueProviderNotSet() {
    }
}
}