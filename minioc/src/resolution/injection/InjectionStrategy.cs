using System.Collections.Generic;
using minioc.resolution.dependencies;

namespace minioc.resolution.injection {
internal interface InjectionStrategy {
    InjectorStrategyType type { get; }
    void inject(object instance, DependencyResolver dependencyResolver, List<Dependency> dependencies);
}
}