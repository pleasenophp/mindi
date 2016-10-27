using System.Collections.Generic;
using System.Reflection;
using minioc.misc;
using MinDI;
using minioc.resolution.dependencies;
using MinDI.StateObjects;
using System;
using MinDI.Resolution;

namespace minioc.resolution.injection {
	internal abstract class BaseInjectionStrategy : IInjectionStrategy {

		public abstract void inject(object instance, IDependencyResolver dependencyResolver, IDependencyResolver explicitDependencies);

		public abstract bool IsVoid();

		protected object ResolveDependency(ResolutionOrder order, bool soft,
			object instance, Type type, string name, 
			IDependencyResolver resolver, IDependencyResolver explicitResolver) 
		{
			object value = null;

			if (explicitResolver != null) {
				value = TryResolveExplicitDependency(type, name, explicitResolver);
			}

			if (value == null && order == ResolutionOrder.FirstExplicitThanContext) {
				value = resolver.TryResolve(type, null, false); 
			}

			if (value == null) {
				if (!soft) {
					ThrowResolutionException(type, name, instance, null);
				}
			}

			return value;
		}

		protected object TryResolveExplicitDependency(Type type, string name, IDependencyResolver explicitResolver) {
			// First try to resolve property-named
			object value = explicitResolver.TryResolve(type, name, true);

			// Then resolve typeless property-named
			if (value == null) {
				value = explicitResolver.TryResolve(typeof(object), name, true);
			}

			// Then resolve typed default
			if (value == null) {
				value = explicitResolver.TryResolve(type, null, true);
			}

			if (value is IDIContext) {
				throw new MiniocException(string.Format("{0} cannot be passed as the explicit dependency !", typeof(IDIContext)));
			}

			return value;
		}
	
		protected void ThrowResolutionException(Type type, string name, object instance, Exception e) {
			throw new MiniocException(string.Format("No binding found for field or parameter of type '{0} named {1}' of '{2}'", type,
				name, instance), e);
		}

	}
}