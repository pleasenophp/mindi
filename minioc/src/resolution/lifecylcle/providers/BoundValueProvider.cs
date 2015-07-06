using System.Collections.Generic;
using minioc.resolution.dependencies;
using minioc.resolution.instantiator;
using minioc.resolution.core;

namespace minioc.resolution.lifecycle.providers {
internal interface BoundValueProvider {
    bool isSet { get; }
    void setInstantiator(Instantiator instantiator);
    object createInstance(InjectionContext injectionContext, List<Dependency> dependencies);
}
}