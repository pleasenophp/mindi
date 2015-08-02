using System;
using System.Collections;
using minioc;
using minioc.context.bindings;
using minioc.resolution.instantiator;
using MinDI.Introspection;


namespace MinDI.Binders {

	public class MultipleBinder : BaseDIBinder
	{

		// TODO - move it to the base binder when singletone generic binding is allowed

		/// <summary>
		/// Binds the generic type definition.
		/// </summary>
		/// <returns>The generic.</returns>
		/// <param name="types">Types.</param>
		public IBinding BindGeneric(Type interfaceType, Type resolutionType, string name = null, Action<IBinding> configure = null) {
			if (!interfaceType.IsGenericTypeDefinition) {
				throw new MindiException(string.Format("The type {0} expected to be a generic type definition", interfaceType));
			}

			if (!resolutionType.IsGenericTypeDefinition) {
				throw new MindiException(string.Format("The type {0} expected to be a generic type definition", resolutionType));
			}


			IBinding binding = InternalBindGeneric(interfaceType, resolutionType, name);
			return RegisterBinding(binding, configure);

		}

		private IBinding InternalBindGeneric(Type interfaceType, Type resolutionType, string name) {
			if (string.IsNullOrEmpty(name)) {
				name = BindHelper.GetDefaultBindingName(interfaceType, context);
			}

			GenericTypeResolver resolver = new GenericTypeResolver(interfaceType, resolutionType);
			return Bindings.ForType(interfaceType, name).ImplementedByInstance(resolver, true)
				.SetDescriptor(this.context, this.instantiationType, BindingType.Instance, null);
		}

		public MultipleBinder(IDIContext context) : base (context) {
			this.instantiationType = InstantiationType.Abstract;
		}
		
		protected override T Resolve<T> (Func<T> create)
		{
			return create ();
		}

	}

}
