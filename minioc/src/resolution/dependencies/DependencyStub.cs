using System;

namespace minioc.resolution.dependencies {
public interface DependencyStub {
    /// <summary>
    /// Will resolve Dependency using the Binding of the parameter type and with given name
    /// </summary>
    /// <param name="instanceName"></param>
    /// <returns></returns>
    Dependency ResolveByName(string instanceName);

    /// <summary>
    /// Will resolve Dependency using the given value
    /// </summary>
    /// <param name="instanceName"></param>
    /// <returns></returns>
    Dependency ResolveByValue(object value);


    /// <summary>
    /// Will resolve Dependency using the given factory (will be call only when the Dependency is resolved
    /// </summary>
    /// <param name="instanceName"></param>
    /// <returns></returns>
    Dependency ResolveByFactory(Func<object> factory);
}
}