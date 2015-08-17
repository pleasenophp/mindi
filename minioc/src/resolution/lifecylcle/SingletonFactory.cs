using System.Collections.Generic;
using minioc.resolution.dependencies;
using minioc.resolution.lifecycle.providers;
using minioc.resolution.core;
using MinDI;

namespace minioc.resolution.lifecycle {

// TODO - fix to support multiple singlrtones, and remove dependencies
internal class SingletonFactory : BoundInstanceFactory {
    private BoundValueProvider _boundValueProvider;
    private object _instance;

    public SingletonFactory(BoundValueProvider boundValueProvider) {
        _boundValueProvider = boundValueProvider;
    }

    public object getInstance(InjectionContext injectionContext) {
        if (_instance != null) {
            return _instance;
        }
        _instance = _boundValueProvider.createInstance(injectionContext);
        return _instance;
    }
}
}