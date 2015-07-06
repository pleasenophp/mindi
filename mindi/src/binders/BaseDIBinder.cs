using System;
using System.Collections;
using minioc;
using minioc.attributes;
using minioc.context.bindings;
using minioc.resolution.instantiator;
using MinDI.Objects;

namespace MinDI.Binders {

	public abstract class BaseDIBinder : PublicContextObject, IDIBinder
	{
		public abstract T Resolve<T> (Func<T> create) where T:class;

		public Binding Bind<T> (Func<T> create, string name = null, Action<Binding> configure = null) where T:class
		{
			Binding binding = InternalBind<T> (create, name);
			if (configure != null) {
				configure (binding);
			} else {
				this.ConfigureBinding (binding);
			}

			context.Register (binding);
			return binding;
		}

		public void BindMany<T1, T2> (Func<object> create, string name = null, Action<Binding> configure = null) 
		where T1:class where T2:class
		{
			Bind<T1> (() => create () as T1, name, configure);
			Bind<T2> (() => create () as T2, name, configure);
		}

		public void BindMany<T1, T2, T3> (Func<object> create, string name = null, Action<Binding> configure = null) 
		where T1:class where T2:class where T3:class
		{
			Bind<T1> (() => create () as T1, name, configure);
			Bind<T2> (() => create () as T2, name, configure);
			Bind<T3> (() => create () as T3, name, configure);
		}

		public void BindMany<T1, T2, T3, T4> (Func<object> create, string name = null, Action<Binding> configure = null)
		where T1:class where T2:class where T3:class where T4:class
		{
			Bind<T1> (() => create () as T1, name, configure);
			Bind<T2> (() => create () as T2, name, configure);
			Bind<T3> (() => create () as T3, name, configure);
			Bind<T4> (() => create () as T4, name, configure);
		}

		public void BindMany<T1, T2, T3, T4, T5> (Func<object> create, string name = null, Action<Binding> configure = null)
		where T1:class where T2:class where T3:class where T4:class where T5:class
		{
			Bind<T1> (() => create () as T1, name, configure);
			Bind<T2> (() => create () as T2, name, configure);
			Bind<T3> (() => create () as T3, name, configure);
			Bind<T4> (() => create () as T4, name, configure);
			Bind<T5> (() => create () as T5, name, configure);
		}

		protected virtual void ConfigureBinding (Binding binding)
		{
			// Setting Multiple as default instantiation mode
			binding.SetInstantiationMode (InstantiationMode.MULTIPLE);
		}

		private Binding InternalBind<T> (Func<T> create, string name=null) where T:class
		{
			return Bindings.ForType<T> (name).ImplementedBy (() => this.Resolve<T> (create));
		}
	}
}
