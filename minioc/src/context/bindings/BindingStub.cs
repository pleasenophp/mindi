using System;

namespace minioc.context.bindings {
public interface BindingStub {
    /// <summary>
    /// Binding will create an instance of given type when resolved
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    Binding ImplementedBy(Type type);

    /// <summary>
    /// Binding will return given instance when resolved
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    Binding ImplementedByInstance(object instance);

    /// <summary>
    /// Binding will return value returned by factory when resolved
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    Binding ImplementedBy(Func<object> factory);
}
}