using System;
using System.Collections.Generic;
using minioc.resolution.dependencies;
using minioc.resolution.instantiator;
using minioc.resolution.lifecycle;
using minioc.resolution.lifecycle.providers;
using minioc.misc;
using minioc.resolution.core;
using MinDI;
using MinDI.Introspection;
using MinDI.StateObjects;

namespace minioc.context.bindings {

	// TODO - add interfaces
	public sealed class Binding {
		const string UNNAMED_BINDING = "__$UNNAMED__";

		public string name { get; set; }
		public IList<Type> types { get; set;}
		public bool isDefault { get; set; }
	
		public InstantiationType instantiationType { get; set; }
		public InstantiationType genericInstantiation { get; set; }
	
		public object instance { get; set;}
		public IDIContext context { get; set; }
		public Func<object> factory { get; set;}

		private Func<object> instantiationFactory;

		private Binding() {
		}

		// TODO - add control checks and exceptions
		// TODO - optimize these 3 methods through constructor
		public static Binding CreateForInstance(IDIContext context, IList<Type> types, object instance,
			 bool isDefault = true, string name = UNNAMED_BINDING) {

			var result = new Binding();
			result.context = context;
			result.types = types;
			result.instance = instance;
			result.instantiationType = InstantiationType.Instance;
			result.genericInstantiation = InstantiationType.None;
			result.factory = null;
			result.name = name;
			result.isDefault = isDefault;
			result.CreateInstantiationFactory();
			return result;
		}

		public static Binding CreateForConcrete(IDIContext context, IList<Type> types, Func<object> factory,
			bool isDefault = true, string name = UNNAMED_BINDING) {

			var result = new Binding();
			result.context = context;
			result.types = types;
			result.instance = null;
			result.instantiationType = InstantiationType.Concrete;
			result.genericInstantiation = InstantiationType.None;
			result.factory = factory;
			result.name = name;
			result.isDefault = isDefault;
			result.CreateInstantiationFactory();
			return result;
		}

		public static Binding CreateForAbstract(IDIContext context, IList<Type> types, Func<object> factory,
			bool isDefault = true, string name = UNNAMED_BINDING) {

			var result = new Binding();
			result.context = context;
			result.types = types;
			result.instance = null;
			result.instantiationType = InstantiationType.Abstract;
			result.genericInstantiation = InstantiationType.None;
			result.factory = factory;
			result.name = name;
			result.isDefault = isDefault;
			result.CreateInstantiationFactory();
			return result;
		}


		public static Binding CreateForGeneric(IDIContext context, IList<Type> types, Type genericType,
			InstantiationType instantiationType, bool isDefault = true, string name = UNNAMED_BINDING) {

			var result = new Binding();
			result.context = context;
			result.types = types;
			result.instance = genericType;
			result.instantiationType = InstantiationType.None;
			result.genericInstantiation = instantiationType;
			result.factory = null;
			result.instantiationFactory = null;
			result.name = name;
			result.isDefault = isDefault;
			return result;
		}

		public static Binding CreateFromGeneric(Binding genericBinding, Type[] genericArguments) {
			var result = new Binding();
			result.context = genericBinding.context;
			result.types = CreateGenericBindingTypes(genericBinding.types, genericArguments);
			result.instantiationType = genericBinding.genericInstantiation;
			result.genericInstantiation = InstantiationType.None;
			result.name = genericBinding.name;
			result.isDefault = genericBinding.isDefault;
			result.factory = CreateGenericBindingFactory(genericBinding.instance as Type, genericArguments);
			result.CreateInstantiationFactory();

			return result;
		}


		public object GetInstance() {
			return instantiationFactory();
		}

		private static IList<Type> CreateGenericBindingTypes(IList<Type> genericTypes, Type[] genericArguments) {
			var result = new List<Type>();
			foreach (Type genericType in genericTypes) {
				result.Add(CreateGenericBindingType(genericType, genericArguments));
			}
			return result;
		}

		private static Type CreateGenericBindingType(Type genericType, Type [] genericArguments) {
			return genericType.MakeGenericType(genericArguments);
		}

		private static Func<object> CreateGenericBindingFactory(Type implementationType, Type[] genericArguments) {
			Type implementorType = CreateGenericBindingType(implementationType, genericArguments);
			Func<object> factory = () => Activator.CreateInstance(implementorType);
			return factory;
		}

		private void CreateInstantiationFactory() {

			switch (this.instantiationType) {
				case InstantiationType.Instance:
					this.instantiationFactory = () => {
						return this.instance;
					};
					break;
				case InstantiationType.Concrete:
					this.instantiationFactory = () => {
						if (this.instance == null) {
							this.instance = this.factory();
						}
						return this.instance;
					};
					break;
				case InstantiationType.Abstract:
					this.instantiationFactory = this.factory;
					break;
			}

		}
	}
}