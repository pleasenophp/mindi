using System;

namespace minioc.resolution.dependencies {
internal interface ExplicitDependencyResolver {
    object resolve(Type type, DependencyResolver dependencyResolver);
}
}