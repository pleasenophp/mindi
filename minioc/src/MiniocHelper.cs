using System;
using System.Collections.Generic;
using minioc.context.bindings;
using minioc.resolution.dependencies;
using minioc.resolution.instantiator;

namespace minioc {
public class MiniocHelper {
    private MiniocContext _context;

    public MiniocHelper(MiniocContext context) {
        _context = new MiniocContext(context);
    }

    public T CreateInstance<T>(List<Dependency> dependencies) {
        return CreateInstance<T>(null, dependencies);
    }

    /// <summary>
    /// Create and injects a new instance of type T with optional dependencies
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instantiator"></param>
    /// <param name="dependencies"></param>
    public T CreateInstance<T>(Instantiator instantiator = null, List<Dependency> dependencies = null) {
        Binding binding = Bindings.ForType<T>().ImplementedBy<T>();
        if (instantiator != null) {
            binding = binding.InstanciatedBy(instantiator);
        }
        if (dependencies != null) {
            foreach (Dependency dependency in dependencies) {
                binding = binding.DependsOn(dependency);
            }
        }
        _context.Register(binding);
        T instance = _context.Resolve<T>();
        _context.RemoveBinding(binding);
        return instance;
    }

    public object CreateInstance(Type type, List<Dependency> dependencies) {
        return CreateInstance(type, dependencies);
    }


    /// <summary>
    /// Create and injects a new instance of given type with optional dependencies
    /// </summary>
    /// <param name="type">Type to instantiate</param>
    /// <param name="instantiator"></param>
    /// <param name="dependencies"></param>
    public object CreateInstance(Type type, Instantiator instantiator = null, List<Dependency> dependencies = null) {
        Binding binding = Bindings.ForConcreteType(type);
        if (instantiator != null) {
            binding = binding.InstanciatedBy(instantiator);
        }
        if (dependencies != null) {
            foreach (Dependency dependency in dependencies) {
                binding = binding.DependsOn(dependency);
            }
        }
        _context.Register(binding);
        object instance = _context.Resolve(type);
        _context.RemoveBinding(binding);
        return instance;
    }
}
}