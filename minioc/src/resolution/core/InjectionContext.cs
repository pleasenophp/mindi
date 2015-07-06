using System;
using System.Collections.Generic;
using minioc.resolution.dependencies;
using minioc.resolution.instantiator;

namespace minioc.resolution.core {
internal interface InjectionContext {
    object createInstance(Type type, Instantiator instantiator, List<Dependency> dependencies);
    void injectDependencies(object instance, List<Dependency> dependencies);
}
}