using System;
using System.Collections.Generic;
using minioc.misc;
using minioc.resolution.dependencies;
using minioc.resolution.injection;
using minioc.resolution.instantiator;
using minioc.resolution.core;
using MinDI;

namespace minioc.context {
	internal class DefaultInjectionContext : InjectionContext {
		private MiniocBindings _bindings;
		private ReflectionCache _reflectionCache;
		private readonly DependencyResolver _dependencyResolver;

		internal DefaultInjectionContext(MiniocBindings bindings, ReflectionCache reflectionCache, DependencyResolver dependencyResolver) {
			_bindings = bindings;
			_reflectionCache = reflectionCache;
			_dependencyResolver = dependencyResolver;
		}

		private InjectionStrategy getInjectionStrategy(Type type) {
			return _reflectionCache.getInjectorStrategy(type);
		}

		internal object resolve(Type type, string name = null) {
			return _bindings.resolve(type, name, this);
		}

		public object createInstance(Type type, Instantiator instantiator, IList<IDependency> dependencies) {
			InjectionStrategy injectionStrategy = getInjectionStrategy(type);
			object instance = null;
		
			// NOTE - this is needed for the class that has no injectors marked. Can inject through default constructor
			// Should be used only as a fail-safe measure
			if (injectionStrategy.type == InjectorStrategyType.CONSTRUCTOR) {
				instance = (injectionStrategy as ConstructorInjectionStrategy).createInstance(_dependencyResolver, dependencies);
			}
			else {
				instance = instantiator.CreateInstance(type);
				injectionStrategy.inject(instance, _dependencyResolver, dependencies);
			}

			return instance;
		}

		public void injectDependencies(object instance, IList<IDependency> dependencies) {
			if (instance == null) {
				throw new MiniocException("Cannot inject dependencies on null value");
			}
		
			// Auto inject feature is removed, as it is implemented better in MinDI
			/*
			IEnumerable<AutoInjectMember> autoInjectMembers = _reflectionCache.getAutoInjectMembers(instance.GetType());
			foreach (AutoInjectMember autoInjectMember in autoInjectMembers) {
				try {
					injectDependencies(autoInjectMember.getValue(instance), dependencies);
				}
				catch (Exception e) {
					throw new MiniocException("Unable to inject dependencies on " + autoInjectMember, e);
				}
			}
			*/
        
			InjectionStrategy injectionStrategy = getInjectionStrategy(instance.GetType());
			if (injectionStrategy.type != InjectorStrategyType.CONSTRUCTOR) {
				injectionStrategy.inject(instance, _dependencyResolver, dependencies);
			}
		}
	}
}