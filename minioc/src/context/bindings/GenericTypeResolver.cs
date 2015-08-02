using System;
using System.Collections.Generic;
using minioc.context.bindings;
using minioc.misc;
using minioc.resolution.core;
using MinDI;
using MinDI.Introspection;

namespace minioc.context.bindings {

	internal class GenericTypeResolver
	{
		private Type baseTypeDefinition;
		private Type resolutionTypeDefinition;

		public GenericTypeResolver(Type baseDef, Type resDef) {
			this.baseTypeDefinition = baseDef;
			this.resolutionTypeDefinition = resDef;
		}

		public BindingImpl CreateBinding(Type baseType, BindingImpl genericBinding) {
			if (baseType.GetGenericTypeDefinition() != this.baseTypeDefinition) {
				throw new MiniocException("Called GenericTypeResolver for a type "+baseType+" that doesn't match the generic type definition: "+baseTypeDefinition);
			}

			Type resolutionType = resolutionTypeDefinition.MakeGenericType(baseType.GetGenericArguments());
			BindingImpl binding = new BindingImpl(resolutionType, genericBinding.name);
			Func<object> factory = () => Activator.CreateInstance(resolutionType);
			binding.InitFromGeneric(genericBinding, factory);
			return binding;
		}

	}

}