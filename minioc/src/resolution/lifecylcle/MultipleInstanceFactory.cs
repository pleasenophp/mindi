using System.Collections.Generic;
using minioc.resolution.dependencies;
using minioc.resolution.lifecycle.providers;
using minioc.resolution.core;
using MinDI;

namespace minioc.resolution.lifecycle {
internal class MultipleInstanceFactory : BoundInstanceFactory {
    private BoundValueProvider _boundValueProvider;

    public MultipleInstanceFactory(BoundValueProvider boundValueProvider) {
        _boundValueProvider = boundValueProvider;
    }

    public object getInstance(InjectionContext injectionContext) {
        return _boundValueProvider.createInstance(injectionContext);
    }
}
}