using System.Collections.Generic;
using minioc.resolution.dependencies;
using minioc.resolution.lifecycle.providers;
using minioc.resolution.core;
using MinDI;

namespace minioc.resolution.lifecycle {
internal class SingletonFactory : BoundInstanceFactory {
    private BoundValueProvider _boundValueProvider;
    private object _instance;

    public SingletonFactory(BoundValueProvider boundValueProvider) {
        _boundValueProvider = boundValueProvider;
    }

    public object getInstance(List<IDependency> dependencies, InjectionContext injectionContext) {
        if (_instance != null) {
            return _instance;
        }
        _instance = _boundValueProvider.createInstance(injectionContext, dependencies);
        return _instance;
    }
}
}