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

		// TODO - check if we need to remove it - if type instance bound value provider should be removed
		// TODO - check the dependencies don't need to be passed here
		public object createInstance(Type type, Instantiator instantiator, IList<IDependency> dependencies) {
			InjectionStrategy injectionStrategy = getInjectionStrategy(type);
			object instance = null;
		
			instance = instantiator.CreateInstance(type);
			// TODO - pass explicit injection context or just null if dependencies are not passed
			injectionStrategy.inject(instance, _dependencyResolver, null);

			/*
			// NOTE - this is needed for the class that has no injectors marked. Can inject through default constructor
			// Should be used only as a fail-safe measure
			if (injectionStrategy.type == InjectorStrategyType.CONSTRUCTOR) {
				instance = (injectionStrategy as ConstructorInjectionStrategy).createInstance(_dependencyResolver, dependencies);
			}
			else {
				instance = instantiator.CreateInstance(type);
				injectionStrategy.inject(instance, _dependencyResolver, dependencies);
			}
			*/

			return instance;
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