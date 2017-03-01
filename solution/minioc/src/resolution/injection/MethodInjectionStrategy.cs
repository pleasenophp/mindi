//using System.Collections.Generic;
//using System.Reflection;
using minioc.misc;
using minioc.resolution.dependencies;
using MinDI;
using System.Collections.Generic;
using System.Reflection;
using MinDI.Resolution;
using MinDI.StateObjects;

namespace minioc.resolution.injection {
	internal class MethodInjectionStrategy : BaseInjectionStrategy {
		private readonly MethodInfo[] _methodInfos;

		public MethodInjectionStrategy(MethodInfo[] methodInfos) {
			_methodInfos = methodInfos;
		}

		public override void inject(object instance, IDependencyResolver dependencyResolver, IDependencyResolver explicitDependencies) {
			foreach (MethodInfo methodInfo in _methodInfos) {
				inject(methodInfo, instance, dependencyResolver, explicitDependencies);
			}
		}

		public override bool IsVoid() {
			return _methodInfos.Length == 0;
		}

		protected void inject(MethodInfo methodInfo, object instance, IDependencyResolver dependencyResolver, IDependencyResolver explicitDependencies) {
			ParameterInfo[] parameterInfos = methodInfo.GetParameters();
			object[] parameterValues = new object[parameterInfos.Length];

			InjectionAttribute atr = (InjectionAttribute)methodInfo.GetCustomAttributes(typeof(InjectionAttribute), true)[0];
			ResolutionOrder order = atr.resolution;
			bool soft = atr.soft;

			for (int i = 0; i < parameterInfos.Length; i++) {
				ParameterInfo parameter = parameterInfos[i];

				object value = ResolveDependency(order, soft, instance,
					parameter.ParameterType, parameter.Name, dependencyResolver, explicitDependencies);

				if (value != null) {
					ControlRemoteObject(instance, value, methodInfo, parameter);
				}

				parameterValues[i] = value;
			}

			methodInfo.Invoke(instance, parameterValues);
		}

		protected void ControlRemoteObject(object instance, object value, MethodInfo methodInfo, ParameterInfo parameterInfo) {
			// Not letting mono behaviours to be injected if they are not IDIClosedContext
			// (because we cannot control their lifetime)

			IDIClosedContext ctxval = value as IDIClosedContext;

			if (RemoteObjectsHelper.IsRemoteObject(value) && (ctxval == null || !ctxval.IsValid())) {
				throw new MiniocException("Injecting a remote object that doesn't implement IDIClosedContext is not allowed ! " +
					"Tried to inject object "+value+" into method "+methodInfo.Name+" into parameter "+parameterInfo.Name
					+" of the object "+instance);
			}
		}

	}
}