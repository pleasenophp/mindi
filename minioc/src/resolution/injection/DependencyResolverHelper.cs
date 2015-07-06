using System;
using System.Collections.Generic;
using System.Linq;
using minioc.resolution.dependencies;

namespace minioc.resolution.injection {
internal static class DependencyResolverHelper {
    public static bool tryResolveDependency(Type type, string name, out object value, DependencyResolver dependencyResolver, IEnumerable<Dependency> dependencies) {
        object tmpValue = null;
        if (dependencies == null) {
            value = null;
            return false;
        }

        bool resolved = dependencies
                .Cast<ExplicitDependency>()
                .Any(dependency => dependency.tryResolveDependency(type, name, out tmpValue, dependencyResolver));
        value = tmpValue;
        return resolved;
    }
}
}