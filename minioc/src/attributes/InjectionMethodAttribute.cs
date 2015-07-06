using System;

namespace minioc.attributes {
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class InjectionMethodAttribute : Attribute {
    public readonly int order;

    public InjectionMethodAttribute(int order = 0) {
        this.order = order;
    }
}
}