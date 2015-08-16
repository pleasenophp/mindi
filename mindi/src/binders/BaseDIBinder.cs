using System;
using System.Collections;
using minioc;
using minioc.context.bindings;
using minioc.resolution.instantiator;
using MinDI.Introspection;

namespace MinDI.Binders {

	public abstract class BaseDIBinder : OpenContextObject
	{
		protected InstantiationType instantiationType;

		public BaseDIBinder(IDIContext context) {
			this.contextInjection = context;
		}

		protected abstract T Resolve<T>(Func<T> create) where T:class;

		/// <summary>
		/// Bind the specified interface using create factory, optional binding name and configuration
		/// </summary>
		/// <param name="create">Create instance factory.</param>
		/// <param name="name">Name for binding.</param>
		/// <param name="configure">Configuration of binding.</param>
		/// <typeparam name="T">The interface type.</typeparam>
		public IBinding Bind<T> (Func<T> create, string name = null, Action<IBinding> configure = null) where T:class
		{
			IBinding binding = InternalBind<T> (create, name);
			return RegisterBinding(binding, configure);
		}
			
		public void BindMany<T1, T2> (Func<object> create, string name = null, Action<IBinding> configure = null) 
		where T1:class where T2:class
		{
			Bind<T1> (() => create () as T1, name, configure);
			Bind<T2> (() => create () as T2, name, configure);
		}

		public void BindMany<T1, T2, T3> (Func<object> create, string name = null, Action<IBinding> configure = null) 
		where T1:class where T2:class where T3:class
		{
			Bind<T1> (() => create () as T1, name, configure);
			Bind<T2> (() => create () as T2, name, configure);
			Bind<T3> (() => create () as T3, name, configure);
		}

		public void BindMany<T1, T2, T3, T4> (Func<object> create, string name = null, Action<IBinding> configure = null)
		where T1:class where T2:class where T3:class where T4:class
		{
			Bind<T1> (() => create () as T1, name, configure);
			Bind<T2> (() => create () as T2, name, configure);
			Bind<T3> (() => create () as T3, name, configure);
			Bind<T4> (() => create () as T4, name, configure);
		}

		public void BindMany<T1, T2, T3, T4, T5> (Func<object> create, string name = null, Action<IBinding> configure = null)
		where T1:class where T2:class where T3:class where T4:class where T5:class
		{
			Bind<T1> (() => create () as T1, name, configure);
			Bind<T2> (() => create () as T2, name, configure);
			Bind<T3> (() => create () as T3, name, configure);
			Bind<T4> (() => create () as T4, name, configure);
			Bind<T5> (() => create () as T5, name, configure);
		}
			
		/// <summary>
		/// Rebind the binding from parent context to a new binder.
		/// This can be usefull to e.g. rebind the library binding to a singletone
		/// </summary>
		/// <param name="name">Name for new binding.</param>
		/// <param name="resolutionName">Resolution name for parent binding.</param>
		/// <param name="configure">Configuration of binding.</param>
		/// <typeparam name="T">The interface type.</typeparam>
		public IBinding Rebind<T>(string name = null, string resolutionName = null, Action<IBinding> configure = null) 
			where T:class 
		{
			if (context.parent == null) {
				throw new MindiException("Called Rebind, but the parent context is null");
			}


			BindingDescriptor descriptor = context.Introspect<T>(resolutionName);
			if (descriptor.instantiationType == InstantiationType.None) {
				throw new MindiException("Called Rebind, but no existing binding found for type "+typeof(T)+" for name "+resolutionName);
			}

			if (descriptor.instantiationType == InstantiationType.Instance) {
				throw new MindiException("Cannot rebind an instance binding. Type: "+typeof(T));
			}

			if (descriptor.context == this.context) {
				throw new MindiException("Called Rebind, but there is already a binding on this context for type "+typeof(T)+
					" for name "+resolutionName);
			}

			return this.Bind<T> (() => descriptor.factory() as T, name, configure);
		}

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
				.SetGenericDescriptor(this.context, InstantiationType.Instance, this.instantiationType);
		}

		protected virtual void ConfigureBinding (IBinding binding)
		{
		}

		private IBinding InternalBind<T> (Func<T> create, string name=null) where T:class
		{
			if (string.IsNullOrEmpty(name)) {
				name = BindHelper.GetDefaultBindingName<T>(context);
			}
				
			return Bindings.ForType<T>(name)
				.ImplementedBy(() => this.Resolve<T>(create))
				.SetDescriptor(this.context, this.instantiationType, () => create());
		}

		protected IBinding RegisterBinding(IBinding binding, Action<IBinding> configure) {
			this.ConfigureBinding (binding);
			if (configure != null) {
				configure (binding);
			} 
			context.Register (binding);
			return binding;
		}
	}
}
