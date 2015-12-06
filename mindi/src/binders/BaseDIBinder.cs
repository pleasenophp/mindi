using System;
using System.Collections;
using minioc;
using minioc.context.bindings;
using MinDI.Introspection;
using System.Collections.Generic;

namespace MinDI.Binders {


	// TODO - mide need to considr making non-generic factory
	public abstract partial class BaseDIBinder : OpenContextObject
	{
		protected InstantiationType instantiationType;

		protected BaseDIBinder(IDIContext context) {
			this.contextInjection = context;
		}

		/// <summary>
		/// Bind the specified interface using create factory, optional binding name and configuration
		/// </summary>
		/// <param name="create">Create instance factory.</param>
		/// <param name="name">Name for binding.</param>
		/// <param name="makeDefault">Make this binding default</param>
		/// <typeparam name="T">The interface type.</typeparam>
		public IBinding Bind<T> (Func<T> create, string name = null, bool makeDefault = false) where T:class
		{
			IBinding binding = CreateBinding<T> (create, name, makeDefault);
			this.context.Register(binding);
			return binding;
		}
			
		/// <summary>
		/// Rebind the binding from parent context to a new binder.
		/// This can be usefull to e.g. rebind the library binding to a singletone
		/// </summary>
		/// <param name="name">Name for new binding.</param>
		/// <param name="resolutionName">Resolution name for parent binding.</param>
		/// <param name="configure">Configuration of binding.</param>
		/// <typeparam name="T">The interface type.</typeparam>
		public IBinding Rebind<T>(string name = null, string resolutionName = null, bool makeDefault = false) 
			where T:class 
		{
			if (context.parent == null) {
				throw new MindiException("Called Rebind, but the parent context is null");
			}

			IBinding binding = context.Introspect<T>(resolutionName);

			if (binding.instantiationType == InstantiationType.None) {
				throw new MindiException("Called Rebind, but no existing binding found for type "+typeof(T)+" for name "+resolutionName);
			}

			if (binding.instantiationType == InstantiationType.Instance) {
				throw new MindiException("Cannot rebind an instance binding. Type: "+typeof(T));
			}

			if (binding.context == this.context) {
				throw new MindiException("Called Rebind, but there is already a binding on this context for type "+typeof(T)+
					" for name "+resolutionName);
			}

			return this.Bind<T> (() => binding.factory() as T, name, makeDefault);
		}

		/// <summary>
		/// Binds the generic type definition.
		/// </summary>
		/// <returns>The generic.</returns>
		/// <param name = "interfaceType"></param>
		/// <param name = "resolutionType"></param>
		/// <param name="types">Types.</param>
		public IBinding BindGeneric(Type interfaceType, Type resolutionType, string name = null, bool makeDefault = false) {
			if (!interfaceType.IsGenericTypeDefinition) {
				throw new MindiException(string.Format("The type {0} expected to be a generic type definition", interfaceType));
			}

			if (!resolutionType.IsGenericTypeDefinition) {
				throw new MindiException(string.Format("The type {0} expected to be a generic type definition", resolutionType));
			}


			IBinding binding = CreateGenericBinding(interfaceType, resolutionType, name, makeDefault);
			context.Register(binding);
			return binding;

		}


		protected IBinding CreateGenericBinding(Type interfaceType, Type resolutionType, string name, bool makeDefault) {
			if (string.IsNullOrEmpty(name)) {
				name = BindHelper.GetDefaultBindingName(interfaceType, context);
			}

			IBinding binding = Binding.CreateForGeneric(context, new List<Type>{ interfaceType }, resolutionType, this.instantiationType, makeDefault, name);
			return binding;
		}


		protected IBinding CreateBinding<T> (Func<T> create, string name, bool makeDefault) {
			if (string.IsNullOrEmpty(name)) {
				name = BindHelper.GetDefaultBindingName<T>(context);
			}

			if (this.instantiationType == InstantiationType.Abstract) {
				return Binding.CreateForAbstract(this.context, new List<Type>{ typeof(T) }, () => create(), makeDefault, name);
			}
			else if (this.instantiationType == InstantiationType.Concrete) {
				return Binding.CreateForConcrete(this.context, new List<Type>{ typeof(T) }, () => create(), makeDefault, name);
			}
			else if (this.instantiationType == InstantiationType.Instance) {
				return Binding.CreateForInstance(this.context, new List<Type>{ typeof(T) }, () => create(), makeDefault, name);
			}
			else {
				throw new MindiException("Unhandled instantiation type: " + this.instantiationType);
			}

		}


	}
}
