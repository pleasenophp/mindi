using minioc.resolution.dependencies;
using minioc.resolution.instantiator;

namespace minioc.context.bindings {
public interface Binding {
    /// <summary>
    /// Specify if the Binding uses the same instance for each 'Resolve' call or if a new instance is created each time
    /// </summary>
    /// <param name="instantiationMode"></param>
    /// <returns></returns>
    Binding SetInstantiationMode(InstantiationMode instantiationMode);

    /// <summary>
    /// Specify the class used to create the instance 
    /// </summary>
    /// <param name="instantiator"></param>
    /// <returns></returns>
    Binding InstanciatedBy(Instantiator instantiator);

    /// <summary>
    /// Add a dependency
    /// </summary>
    /// <param name="dependency"></param>
    /// <returns></returns>
    Binding DependsOn(Dependency dependency);

    /// <summary>
    /// Make this binding the default binding for the bound type
    /// </summary>
    Binding MakeDefault();

}
}