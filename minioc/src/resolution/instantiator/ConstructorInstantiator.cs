using System;

namespace minioc.resolution.instantiator {
internal class ConstructorInstantiator : Instantiator {
    public static readonly Instantiator INSTANCE = new ConstructorInstantiator();

    public object CreateInstance(Type type) {
        return Activator.CreateInstance(type);
    }

    public bool AllowConstructorInjection() {
        return true;
    }
}
}