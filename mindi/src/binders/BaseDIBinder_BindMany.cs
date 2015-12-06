using System;
using System.Collections;
using minioc;
using minioc.context.bindings;
using MinDI.Introspection;

namespace MinDI.Binders {

	// TODO - use universal core acceptor for many types
	public abstract partial class BaseDIBinder
	{		
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
	}
}
