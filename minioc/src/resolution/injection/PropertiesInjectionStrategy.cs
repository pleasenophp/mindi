using System.Collections.Generic;
using System.Reflection;
using minioc.misc;
using MinDI;
using minioc.resolution.dependencies;

namespace minioc.resolution.injection {
internal class PropertiesInjectionStrategy : InjectionStrategy {
    private readonly PropertyInfo[] _properties;

    public PropertiesInjectionStrategy(PropertyInfo[] properties) {
        _properties = properties;
    }

    public InjectorStrategyType type { get { return InjectorStrategyType.PROPERTIES; } }

    public void inject(object instance, DependencyResolver dependencyResolver, IList<IDependency> dependencies) {
        foreach (PropertyInfo propertyInfo in _properties) {
            object value = null;

            try {
                bool resolved = DependencyResolverHelper.tryResolveDependency(propertyInfo.PropertyType,
                                                                              propertyInfo.Name,
                                                                              out value, dependencyResolver,
                                                                              dependencies);
                if (!resolved) {
                    value = dependencyResolver.Resolve(propertyInfo.PropertyType);
                }
            } catch (MiniocException e) {
                throw new MiniocException(string.Format("No binding found for property '{0} {1}' of '{2}'", propertyInfo.PropertyType,
                    propertyInfo.Name, propertyInfo.DeclaringType), e);
			}

            propertyInfo.SetValue(instance, value, null);
        }
    }
}
}