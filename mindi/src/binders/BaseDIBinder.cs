using System;
using System.Collections;
using minioc;
using minioc.context.bindings;
using minioc.resolution.instantiator;
using MinDI.Objects;

namespace MinDI.Binders {

	public abstract class BaseDIBinder : PublicContextObject, IDIBinder
	{
		public abstract T Resolve<T> (Func<T> create) where T:class;

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
			this.ConfigureBinding (binding);
			if (configure != null) {
				configure (binding);
			} 

			context.Register (binding);
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
		public IBinding Rebind<T>(string name = null, string resolutionName = null, Action<IBinding> configure = null) 
			where T:class 
		{
			if (context.parent == null) {
				throw new MindiException("Called Rebind, but the parent context is null");
			}

			return this.Bind<T> (()=>context.parent.Resolve<T>(resolutionName, true), 
				name, configure);
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
				string contextName = (string.IsNullOrEmpty(context.name))?"context":context.name;
				name = string.Format("{0}_{1}_{2}", "#binding", contextName, typeof(T).FullName);
			}

			return Bindings.ForType<T> (name).ImplementedBy (() => this.Resolve<T> (create));
		}
	}
}
