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

		private InjectionStrategy getInjectionStrategy(Type type) {
			return _reflectionCache.getInjectorStrategy(type);
		}

		public void injectDependencies(object instance, IConstruction construction) {
			if (instance == null) {
				throw new MiniocException("Cannot inject dependencies on null value");
			}
        
			InjectionStrategy injectionStrategy = getInjectionStrategy(instance.GetType());
			if (injectionStrategy.type != InjectorStrategyType.CONSTRUCTOR) {
				injectionStrategy.inject(instance, _dependencyResolver, construction!=null?construction.GetExplicitContext():null);
			}
		}
	}
}