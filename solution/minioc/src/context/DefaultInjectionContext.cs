using System;
using System.Collections.Generic;
using minioc.misc;
using minioc.resolution.dependencies;
using minioc.resolution.injection;
using minioc.resolution.core;
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

	    public IList<IInjectionStrategy> getInjectionStrategies(object instance) {
			IList<IInjectionStrategy> injectionStrategies = getInjectionStrategies(instance.GetType());
	        return injectionStrategies;
	    }

		public void injectDependencies(object instance, IList<IInjectionStrategy> injectionStrategies, Func<IConstruction> construction) {
			foreach (IInjectionStrategy strategy in injectionStrategies) {
				strategy.inject(instance, _dependencyResolver, construction != null?construction().GetExplicitContext():null);
			}
		}
	}
}