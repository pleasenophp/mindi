using System;

namespace minioc.resolution.dependencies {
internal class NameDependencyTarget : DependencyTarget {
    private readonly string _name;

    public NameDependencyTarget(string name) {
        _name = name;
    }

    public bool accept(Type type, string name) {
        return name == _name;
    }
}
}