using System.Collections.Generic;
using System.Reflection;
using minioc.misc;
using MinDI;
using minioc.resolution.dependencies;
using MinDI.StateObjects;
using System;
using MinDI.Resolution;

namespace minioc.resolution.injection {
	internal class PropertiesInjectionStrategy : BaseInjectionStrategy {
		private readonly PropertyInfo[] _properties;

		public PropertiesInjectionStrategy(PropertyInfo[] properties) {
			_properties = properties;
		}

		public override void inject(object instance, IDependencyResolver dependencyResolver, IDependencyResolver explicitDependencies) {
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

		public override bool IsVoid() {
			return _properties.Length == 0;
		}

		protected void ControlRemoteObject(object instance, object value, PropertyInfo propertyInfo) {
			// Not letting mono behaviours to be injected if they are not IDIClosedContext
			// (because we cannot control their lifetime)

			if (RemoteObjectsHelper.IsRemoteObject(value) && !(value is IDIClosedContext)) {
				throw new MiniocException("Injecting a remote object that doesn't implement IDIClosedContext is not allowed ! Tried to inject object "+value+" into property "+propertyInfo.Name+" of the object "+instance);
			}
		}
	}
}