using System;
using System.Collections;
using minioc;
using minioc.context.bindings;
using minioc.resolution.instantiator;

namespace MinDI.Binders {

	public abstract class BaseDIBinder : OpenContextObject, IDIBinder
	{
		public BaseDIBinder(IDIContext context) {
			this.context = context;
		}

		protected abstract T Resolve<T> (Func<T> create) where T:class;

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


		public IBinding BindInstance<T> (T instance, string name = null, Action<IBinding> configure = null) {
			IBinding binding = InternalBindInstance<T>(instance, name);
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

		protected virtual void ConfigureBinding (IBinding binding)
		{
			// Setting Multiple as default instantiation mode
			binding.SetInstantiationMode (InstantiationMode.MULTIPLE);
		}

		private IBinding InternalBind<T> (Func<T> create, string name=null) where T:class
		{
			if (string.IsNullOrEmpty(name)) {
				name = BindHelper.GetDefaultBindingName<T>(context);
			}

			return Bindings.ForType<T> (name).ImplementedBy (() => this.Resolve<T> (create));
		}

		private IBinding InternalBindInstance<T> (T instance, string name=null)
		{
			if (string.IsNullOrEmpty(name)) {
				name = BindHelper.GetDefaultBindingName<T>(context);
			}

			return Bindings.ForType<T> (name).ImplementedByInstance(instance);
		}

		private IBinding RegisterBinding(IBinding binding, Action<IBinding> configure) {
			this.ConfigureBinding (binding);
			if (configure != null) {
				configure (binding);
			} 
			context.Register (binding);
			return binding;
		}
	}
}
