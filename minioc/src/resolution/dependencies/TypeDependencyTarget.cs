using System;

namespace minioc.resolution.dependencies {
internal class TypeDependencyTarget : DependencyTarget {
    private readonly Type _type;

    public TypeDependencyTarget(Type type) {
        _type = type;
    }

    public bool accept(Type type, string name) {
        return type == _type;
    }
}
}