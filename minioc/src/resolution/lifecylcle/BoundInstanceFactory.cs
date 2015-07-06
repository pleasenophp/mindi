using System.Collections.Generic;
using minioc.resolution.dependencies;
using minioc.resolution.core;

namespace minioc.resolution.lifecycle {
internal interface BoundInstanceFactory {
    object getInstance(List<Dependency> dependencies, InjectionContext injectionContext);
}
}