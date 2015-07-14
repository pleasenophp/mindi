using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using minioc.resolution.injection;
using minioc.misc;
using MinDI;

namespace minioc.context {
	internal class ReflectionCache {
		private Dictionary<Type, InjectionStrategy> _injectionStrategies = new Dictionary<Type, InjectionStrategy>();
		private Dictionary<Type, List<AutoInjectMember>> _autoInjectMembersByType = new Dictionary<Type, List<AutoInjectMember>>();

		internal InjectionStrategy getInjectorStrategy(Type type) {
			InjectionStrategy stategy;
			if (!_injectionStrategies.TryGetValue(type, out stategy)) {
				stategy = createInjectionStrategy(type);
				_injectionStrategies.Add(type, stategy);
			}
			return stategy;
		}

		private InjectionStrategy createInjectionStrategy(Type type) {
			if (type.IsAbstract) {
				throw new MiniocException(string.Format("Type {0} is abstract, cannot instantiate", type));
			}
			else if (type.IsInterface) {
				throw new MiniocException(string.Format("Type {0} is an interface, cannot instantiate", type));
			}

			InjectionStrategy injectionStrategy = tryInjectProperties(type);
			if (injectionStrategy == null) {
				injectionStrategy = tryInjectMethod(type);
			}
			if (injectionStrategy == null) {
				List<ConstructorInfo> constructors = type.GetConstructors().ToList();
				injectionStrategy = tryInjectUnTaggedConstructor(type, constructors);
			}

			return injectionStrategy;


			/*
	        List<ConstructorInfo> constructors = type.GetConstructors().ToList();
	        InjectionStrategy injectionStrategy = tryInjectTaggedConstructor(constructors);
	        if (injectionStrategy == null) {
	            injectionStrategy = tryInjectMethod(type);
	        }
	        if (injectionStrategy == null) {
	            injectionStrategy = tryInjectProperties(type);
	        }
	        if (injectionStrategy == null) {
	            injectionStrategy = tryInjectUnTaggedConstructor(type, constructors);
	        }
	        return injectionStrategy;
	        */
		}
			

		private InjectionStrategy tryInjectMethod(Type type) {
			List<MethodInfo> methodInfos = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				.Where(m => m.GetCustomAttributes(typeof(InjectionMethodAttribute), false).Any()).ToList();
			if (methodInfos.Count > 0) {
				methodInfos.Sort(injectionOrderSorter);
				return new MethodInjectionStrategy(methodInfos);
			}
			return null;
		}

		private static InjectionStrategy tryInjectProperties(Type type) {
			PropertyInfo[] properties =
				type.GetProperties(BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			PropertyInfo[] injectedProperties =
				properties.Where(p => p.GetCustomAttributes(typeof(InjectionAttribute), false).Any()).ToArray();
			if (injectedProperties.Length > 0) {
				return new PropertiesInjectionStrategy(injectedProperties);
			}
			return null;
		}

		private static InjectionStrategy tryInjectUnTaggedConstructor(Type type, List<ConstructorInfo> constructors) {
			if (constructors.Count == 0) {
				throw new MiniocException(
					string.Format("Type '{0}' doesn't have public constructor nor injection attributes, cannot instanciate",
						type));
			}
			constructors.Sort((a, b) => a.GetParameters().Length.CompareTo(b.GetParameters().Length));
			return new ConstructorInjectionStrategy(constructors[0]);
		}

		// Don't wanna allow to inject constructors as it will not work with the current implementation
		/*
		private static InjectionStrategy tryInjectTaggedConstructor(List<ConstructorInfo> constructors) {
			ConstructorInfo[] injectConstructors =
				constructors.Where(c => c.GetCustomAttributes(typeof(InjectionConstructorAttribute), false).Any()).ToArray();
			if (injectConstructors.Length == 1) {
				return new ConstructorInjectionStrategy(injectConstructors[0]);
			}
			else if (injectConstructors.Length > 1) {
				throw new MiniocException("Only 1 constuctor may be marked with [InjectConstructor]");
			}
			return null;
		}
		 
		*/

		private int injectionOrderSorter(MethodInfo a, MethodInfo b) {
			InjectionMethodAttribute injectionMethodAttributeA = (InjectionMethodAttribute)a.GetCustomAttributes(typeof(InjectionMethodAttribute), false)[0];
			InjectionMethodAttribute injectionMethodAttributeB = (InjectionMethodAttribute)b.GetCustomAttributes(typeof(InjectionMethodAttribute), false)[0];
			return injectionMethodAttributeA.order.CompareTo(injectionMethodAttributeB.order);
		}

		public IEnumerable<AutoInjectMember> getAutoInjectMembers(Type type) {
			List<AutoInjectMember> autoInjectMembers;
			if (!_autoInjectMembersByType.TryGetValue(type, out autoInjectMembers)) {
				autoInjectMembers = findAutoInjectMembers(type);
				_autoInjectMembersByType[type] = autoInjectMembers;
			}
			return autoInjectMembers;
		}

		private List<AutoInjectMember> findAutoInjectMembers(Type type) {
			MemberInfo[] members = type.FindMembers(MemberTypes.Field | MemberTypes.Property, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, hasAutoInjectAttribute, null);
			return members.Select<MemberInfo, AutoInjectMember>(createAutoInjectMember).ToList();
			// return members.Select(createAutoInjectMember).ToList();
		}

		private AutoInjectMember createAutoInjectMember(MemberInfo x) {
			return x.MemberType == MemberTypes.Field?(AutoInjectMember)new AutoInjectField((FieldInfo)x):new AutoInjectProperty((PropertyInfo)x);
		}

		private bool hasAutoInjectAttribute(MemberInfo m, object filtercriteria) {
			return m.GetCustomAttributes(typeof(AutoInjectAttribute), true).Length == 1;
		}
	}
}