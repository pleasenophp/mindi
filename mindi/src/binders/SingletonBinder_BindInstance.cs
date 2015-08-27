using System;
using System.Collections;
using minioc;

using minioc.context.bindings;
using minioc.resolution.instantiator;
using MinDI.StateObjects;
using MinDI.Introspection;

namespace MinDI.Binders {

	// That's usefull for the singleton classes 
	public partial class SingletonBinder
	{
		public IBinding BindInstance<T> (T instance, string name = null, Action<IBinding> configure = null) {
			IBinding binding = InternalBindInstance<T>(instance, name);
			return RegisterBinding(binding, configure);
		}

		public void BindInstance<T1, T2> (object instance, string name = null, Action<IBinding> configure = null) 
			where T1:class where T2:class 
		{
			BindInstance<T1>(instance as T1, name, configure);
			BindInstance<T2>(instance as T2, name, configure);
		}

		public void BindInstance<T1, T2, T3> (object instance, string name = null, Action<IBinding> configure = null) 
			where T1:class where T2:class where T3:class 
		{
			BindInstance<T1>(instance as T1, name, configure);
			BindInstance<T2>(instance as T2, name, configure);
			BindInstance<T3>(instance as T3, name, configure);
		}

		public void BindInstance<T1, T2, T3, T4> (object instance, string name = null, Action<IBinding> configure = null) 
			where T1:class where T2:class where T3:class where T4:class 
		{
			BindInstance<T1>(instance as T1, name, configure);
			BindInstance<T2>(instance as T2, name, configure);
			BindInstance<T3>(instance as T3, name, configure);
			BindInstance<T4>(instance as T4, name, configure);
		}

		public void BindInstance<T1, T2, T3, T4, T5> (object instance, string name = null, Action<IBinding> configure = null) 
			where T1:class where T2:class where T3:class where T4:class where T5:class
		{
			BindInstance<T1>(instance as T1, name, configure);
			BindInstance<T2>(instance as T2, name, configure);
			BindInstance<T3>(instance as T3, name, configure);
			BindInstance<T4>(instance as T4, name, configure);
			BindInstance<T5>(instance as T5, name, configure);
		}
	}
}
