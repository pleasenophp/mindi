using System.Collections.Generic;
using System.Reflection;
using minioc.misc;
using MinDI;
using minioc.resolution.dependencies;
using MinDI.StateObjects;
using System;
using MinDI.Resolution;

namespace minioc.resolution.injection {
	internal class PropertiesInjectionStrategy : InjectionStrategy {
		private readonly PropertyInfo[] _properties;

		public PropertiesInjectionStrategy(PropertyInfo[] properties) {
			_properties = properties;
		}

		public InjectorStrategyType type { get { return InjectorStrategyType.PROPERTIES; } }

		public void inject(object instance, IDependencyResolver dependencyResolver, IDependencyResolver explicitDependencies) {
			foreach (PropertyInfo propertyInfo in _properties) {
				InjectionAttribute atr = (InjectionAttribute)propertyInfo.GetCustomAttributes(typeof(InjectionAttribute), true)[0];
				ResolutionOrder order = atr.resolution;
				bool soft = atr.soft;

				object value = ResolveDependency(order, soft, instance,
					propertyInfo.PropertyType, propertyInfo.Name, dependencyResolver, explicitDependencies);
				
				if (value != null) {
					ControlRemoteObject(instance, value, propertyInfo);
					propertyInfo.SetValue(instance, value, null);
				}
			}
		}
			
		private object ResolveDependency(ResolutionOrder order, bool soft,
			object instance, Type type, string name, 
			IDependencyResolver resolver, IDependencyResolver explicitResolver) 
		{
			object value = null;

			if (explicitResolver != null) {
				value = TryResolveExplicitDependency(type, name, explicitResolver);
			}

			if (value == null && order == ResolutionOrder.FirstExplicitThanContext) {
				value = resolver.TryResolve(type); 
			}

			if (value == null) {
				if (!soft) {
					ThrowResolutionException(type, name, instance, null);
				}
			}

			return value;
		}

		private object TryResolveExplicitDependency(Type type, string name, IDependencyResolver explicitResolver) {
			// First try to resolve with the name of property
			object value = explicitResolver.TryResolve(type, name);

			// Then try resolve by type
			if (value == null) {
				value = explicitResolver.TryResolve(type);
			}

			return value;
		}
	
		private void ThrowResolutionException(Type type, string name, object instance, Exception e) {
			throw new MiniocException(string.Format("No binding found for property '{0} {1}' of '{2}'", type,
				name, instance), e);
		}


		private void ControlRemoteObject(object instance, object value, PropertyInfo propertyInfo) {
			// NOTE - this can be removed if the TODO in MiniocContext is done - so we workaround non-context monobehaviours
			if (RemoteObjectsHelper.IsRemoteObject(value) && !(value is IDIClosedContext)) {
				throw new MiniocException("Injecting a remote object that doesn't implement IDIClosedContext is not allowed ! Tried to inject object "+value+" into property "+propertyInfo.Name+" of the object "+instance);
			}
		}
	}
}