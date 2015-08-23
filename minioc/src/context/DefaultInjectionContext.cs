using System;
using System.Collections.Generic;
using minioc.misc;
using minioc.resolution.dependencies;
using minioc.resolution.injection;
using minioc.resolution.instantiator;
using minioc.resolution.core;
using MinDI;
using MinDI.Resolution;

namespace minioc.context {
	internal class DefaultInjectionContext : InjectionContext {
		private ReflectionCache _reflectionCache;
		private readonly IDependencyResolver _dependencyResolver;

		internal DefaultInjectionContext(ReflectionCache reflectionCache, IDependencyResolver dependencyResolver) {
			_reflectionCache = reflectionCache;
			_dependencyResolver = dependencyResolver;
		}

		private IList<IInjectionStrategy> getInjectionStrategies(Type type) {
			return _reflectionCache.getInjectorStrategies(type);
		}

		public void injectDependencies(object instance, Func<IConstruction> construction) {
			if (instance == null) {
				throw new MiniocException("Cannot inject dependencies on null value");
			}
        
			IList<IInjectionStrategy> injectionStrategies = getInjectionStrategies(instance.GetType());

			foreach (IInjectionStrategy strategy in injectionStrategies) {
				strategy.inject(instance, _dependencyResolver, construction != null?construction().GetExplicitContext():null);
			}


		}
	}
}