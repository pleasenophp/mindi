//using System;
//
//namespace minioc.resolution.dependencies {
//internal class ValueExplicitDependencyResolver : ExplicitDependencyResolver {
//    private readonly object _value;
//
//    public ValueExplicitDependencyResolver(object value) {
//        _value = value;
//    }
//
//    public object resolve(Type type, IDependencyResolver dependencyResolver) {
//        return _value;
//    }
//}
//}