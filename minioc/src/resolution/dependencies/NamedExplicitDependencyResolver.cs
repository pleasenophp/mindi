using System;

namespace minioc.resolution.dependencies {
internal class NamedExplicitDependencyResolver : ExplicitDependencyResolver {
    private readonly string _instanceName;

    public NamedExplicitDependencyResolver(string instanceName) {
        _instanceName = instanceName;
    }

    public object resolve(Type type, DependencyResolver dependencyResolver) {
        return dependencyResolver.Resolve(type, _instanceName);
    }
}
}