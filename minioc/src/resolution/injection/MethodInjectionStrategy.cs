//using System.Collections.Generic;
//using System.Reflection;
//using minioc.misc;
//using minioc.resolution.dependencies;
//using MinDI;
//
//// TODO - if keeping this, check for the mono behaviour as in property
//namespace minioc.resolution.injection {
//	internal class MethodInjectionStrategy : InjectionStrategy {
//		private readonly List<MethodInfo> _methodInfos;
//
//		public MethodInjectionStrategy(List<MethodInfo> methodInfos) {
//			_methodInfos = methodInfos;
//		}
//
//		public InjectorStrategyType type { get { return InjectorStrategyType.METHOD; } }
//
//		public void inject(object instance, IDependencyResolver dependencyResolver, IList<IDependency> dependencies) {
//			foreach (MethodInfo methodInfo in _methodInfos) {
//				inject(methodInfo, instance, dependencyResolver, dependencies);
//			}
//		}
//
//		private void inject(MethodInfo methodInfo, object instance, IDependencyResolver dependencyResolver, IList<IDependency> dependencies) {
//			ParameterInfo[] parameterInfos = methodInfo.GetParameters();
//			object[] parameterValues = new object[parameterInfos.Length];
//
//			for (int i = 0; i < parameterInfos.Length; i++) {
//				try {
//					object value;
//					bool resolved = DependencyResolverHelper.tryResolveDependency(parameterInfos[i].ParameterType,
//						                           parameterInfos[i].Name,
//						                           out value, dependencyResolver,
//						                           dependencies);
//					if (!resolved) {
//						value = dependencyResolver.Resolve(parameterInfos[i].ParameterType);
//					}
//					parameterValues[i] = value;
//				}
//				catch (MiniocException e) {
//					throw new MiniocException(
//						string.Format("No binding found for parameter '{0} {1}' of method of '{2}'",
//							parameterInfos[i].ParameterType,
//							parameterInfos[i].Name, methodInfo.DeclaringType), e);
//				}
//			}
//
//			methodInfo.Invoke(instance, parameterValues);
//		}
//	}
//}