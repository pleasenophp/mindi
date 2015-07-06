using System;
using minioc.misc;

namespace minioc.context.bindings {
public class GenericBindingStub<T> {
    private string _name;

    internal GenericBindingStub(string name) {
        _name = name;
    }

    /// <summary>
    /// Binding will create an instance of type U when resolved
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <returns></returns>
    public Binding ImplementedBy<U>() where U : T {
        return new BindingImpl(typeof(T), _name).ImplementedBy<U>();
    }

    /// <summary>
    /// Binding will create an instance of given type when resolved.
    /// Usage of this method is discouraged unless you are already manipulating Type objects
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Binding ImplementedBy(Type type) {
        return new BindingImpl(typeof(T), _name).ImplementedBy(type);
    }

    /// <summary>
    /// Binding will return given instance when resolved
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    public Binding ImplementedByInstance(T instance) {
        return new BindingImpl(typeof(T), _name).ImplementedByInstance(instance);
    }

    /// <summary>
    /// Binding will return value returned by factory when resolved
    /// </summary>
    /// <param name="factory"></param>
    /// <returns></returns>
    public Binding ImplementedBy(Func<T> factory) {
        return new BindingImpl(typeof(T), _name).ImplementedBy(() => factory());
    }
}
}