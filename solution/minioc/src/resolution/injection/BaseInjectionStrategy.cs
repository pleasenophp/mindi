using minioc.misc;
using MinDI;
using minioc.resolution.dependencies;
using System;
using MinDI.Resolution;

namespace minioc.resolution.injection {
	internal abstract class BaseInjectionStrategy : IInjectionStrategy {
		public abstract void inject(object instance, IDependencyResolver dependencyResolver, IDependencyResolver explicitDependencies);

		public abstract bool IsVoid();

		protected object ResolveDependency(ResolutionOrder order, bool soft,
			object instance, Type type, string name,
			IDependencyResolver resolver, IDependencyResolver explicitResolver) {
			object value = null;

			if (explicitResolver != null) {
				value = TryResolveExplicitDependency(type, name, explicitResolver);
			}

			if (value == null && order == ResolutionOrder.FirstExplicitThanContext) {
				value = resolver.TryResolve(type, null, false);
			}

			if (value != null) {
				return value;
			}
			if (!soft) {
				ThrowResolutionException(type, name, instance, null);
			}

			return value;
		}

		protected object TryResolveExplicitDependency(Type type, string name, IDependencyResolver explicitResolver) {
			// First try to resolve property-named, then resolve typeless property-named
			var value = (explicitResolver.TryResolve(type, name, true) ?? explicitResolver.TryResolve(typeof(object), name, true)) ?? explicitResolver.TryResolve(type, null, true);

			// Then resolve typed default
			if (value is IDIContext) {
				throw new MiniocException(string.Format("{0} cannot be passed as the explicit dependency !", typeof(IDIContext)));
			}

			return value;
		}

		protected static void ThrowResolutionException(Type type, string name, object instance, Exception e) {
			throw new MiniocException(
				string.Format("No binding found for field or parameter of type '{0}', named '{1}' in the instance '{2}'", type,
					name, instance), e);
		}
	}
}