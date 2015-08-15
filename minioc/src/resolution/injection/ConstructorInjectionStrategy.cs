using System.Collections.Generic;
using System.Reflection;
using minioc.misc;
using minioc.resolution.dependencies;
using MinDI;

namespace minioc.resolution.injection {
	internal class ConstructorInjectionStrategy : InjectionStrategy {
		private readonly ConstructorInfo _constructorInfo;

		public ConstructorInjectionStrategy(ConstructorInfo constructorInfo) {
			_constructorInfo = constructorInfo;
		}

		public object createInstance(IDependencyResolver dependencyResolver, IList<IDependency> dependencies) {
			ParameterInfo[] parameterInfos = _constructorInfo.GetParameters();
			object[] parameterValues = new object[parameterInfos.Length];

			for (int i = 0; i < parameterInfos.Length; i++) {
				try {
					object value;
					bool resolved = DependencyResolverHelper.tryResolveDependency(parameterInfos[i].ParameterType,
						                           parameterInfos[i].Name, out value,
						                           dependencyResolver, dependencies);
					if (!resolved) {
						value = dependencyResolver.Resolve(parameterInfos[i].ParameterType);
					}
					parameterValues[i] = value;
				}
				catch (MiniocException e) {
					throw new MiniocException(string.Format("No binding found for parameter '{0} {1}' of constructor of '{2}'", parameterInfos[i].ParameterType,
						parameterInfos[i].Name, _constructorInfo.DeclaringType), e);
				}
			}

			return _constructorInfo.Invoke(parameterValues);
		}

		public InjectorStrategyType type { get { return InjectorStrategyType.CONSTRUCTOR; } }

		public void inject(object instance, IDependencyResolver dependencyResolver, IList<IDependency> dependencies) {
			throw new System.NotImplementedException();
		}
	}
}