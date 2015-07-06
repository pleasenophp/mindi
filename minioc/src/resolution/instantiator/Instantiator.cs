using System;

namespace minioc.resolution.instantiator {
public interface Instantiator {
    object CreateInstance(Type type);
    bool AllowConstructorInjection();
}
}