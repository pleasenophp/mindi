using System;
using System.Collections;
using minioc;
using minioc.attributes;
using minioc.context.bindings;
using minioc.resolution.instantiator;


namespace MinDI.Binders {

	public interface IDIBinder
	{
		T Resolve<T> (Func<T> create) where T:class;

		Binding Bind<T> (Func<T> create, string name = null, Action<Binding> configure = null) where T:class;

		void BindMany<T1, T2> (Func<object> create, string name = null, Action<Binding> configure = null) 
			where T1:class where T2:class;

		void BindMany<T1, T2, T3> (Func<object> create, string name = null, Action<Binding> configure = null) 
			where T1:class where T2:class where T3:class;

		void BindMany<T1, T2, T3, T4> (Func<object> create, string name = null, Action<Binding> configure = null) 
			where T1:class where T2:class where T3:class where T4:class;

		void BindMany<T1, T2, T3, T4, T5> (Func<object> create, string name = null, Action<Binding> configure = null) 
			where T1:class where T2:class where T3:class where T4:class where T5:class;
	}
}