using System;
using MinDI;

namespace minioc.resolution.instantiator {

	// TODO - remove if not used together with Instantiator interface
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