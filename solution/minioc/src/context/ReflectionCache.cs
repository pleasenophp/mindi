using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using minioc.resolution.injection;
using minioc.misc;
using MinDI;
using MinDI.StateObjects;

namespace minioc.context {
	internal class ReflectionCache {
		private Dictionary<Type, IList<IInjectionStrategy>> _injectionStrategies = new Dictionary<Type, IList<IInjectionStrategy>>();

		internal IList<IInjectionStrategy> getInjectorStrategies(Type type) {
			IList<IInjectionStrategy> strategies;
			if (!_injectionStrategies.TryGetValue(type, out strategies)) {
				strategies = createInjectionStrategies(type);
				_injectionStrategies[type] = strategies;
			}
			return strategies;
		}

		private IList<IInjectionStrategy> createInjectionStrategies(Type type) {
			var result = new List<IInjectionStrategy>();

			if (type.IsAbstract) {
				throw new MiniocException(string.Format("Type {0} is abstract, cannot instantiate", type));
			}
			else if (type.IsInterface) {
				throw new MiniocException(string.Format("Type {0} is an interface, cannot instantiate", type));
			}
			else if (type.IsPrimitive || type.IsEnum) {
				return new List<IInjectionStrategy>{new PrimitiveInjectionStrategy()};
			}

			IInjectionStrategy propertiesStrategy = tryInjectProperties(type);
			if (!propertiesStrategy.IsVoid()) {
				result.Add(propertiesStrategy);	
			}

			IInjectionStrategy methodsStrategy = tryInjectMethods(type);
			if (!methodsStrategy.IsVoid()) {
				result.Add(methodsStrategy);	
			}

			return result;
		}
			
		private static IInjectionStrategy tryInjectProperties(Type type) {
			PropertyInfo[] properties =
				type.GetProperties(BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			PropertyInfo[] injectedProperties =
				properties.Where(p => p.GetCustomAttributes(typeof(InjectionAttribute), true).Any()).ToArray();

		    if (injectedProperties.Length > 0 && !typeof(IDIClosedContext).IsAssignableFrom(type))
		    {
		        throw new MiniocException(string.Format("The type {0} contains Injection properties, " +
		                                                "but is not implementing IDIClosedContext. Maybe you forgot to derive it from ContextObject or similar.", type));
		    }
			
			return new PropertiesInjectionStrategy(injectedProperties);
		}
			
		private IInjectionStrategy tryInjectMethods(Type type) {
			MethodInfo[] methodInfos = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				.Where(m => m.GetCustomAttributes(typeof(InjectionAttribute), true).Any()).ToArray();

			// Call sort if order is introduced
			// methodInfos.Sort(injectionOrderSorter);

		    if (methodInfos.Length > 0 && !typeof(IDIClosedContext).IsAssignableFrom(type))
		    {
		        throw new MiniocException(string.Format("The type {0} contains Injection methods, " +
		                                                "but is not implementing IDIClosedContext. Maybe you forgot to derive it from ContextObject or similar.", type));
		    }

			return new MethodInjectionStrategy(methodInfos);
		}

		// TODO - reenable if we introduce order and add to properties and methods
		/*
		private int injectionOrderSorter(MethodInfo a, MethodInfo b) {
			var injectionMethodAttributeA = (InjectionAttribute)a.GetCustomAttributes(typeof(InjectionAttribute), true)[0];
			var injectionMethodAttributeB = (InjectionAttribute)b.GetCustomAttributes(typeof(InjectionAttribute), true)[0];
			return injectionMethodAttributeA.order.CompareTo(injectionMethodAttributeB.order);
		}
		*/
			
	}
}