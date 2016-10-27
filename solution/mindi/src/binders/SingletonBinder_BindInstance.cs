using System;
using System.Collections;
using System.Collections.Generic;
using minioc;

using MinDI.StateObjects;
using MinDI.Introspection;

namespace MinDI.Binders {

	// That's usefull for the singleton classes 
	public partial class SingletonBinder
	{
		public IBinding BindInstance<T> (T instance, string name = null, bool makeDefault = false) {
			return Bind(InstantiationType.Instance, new List<Type>{typeof(T)}, () => instance, name, makeDefault);
		}

		public IBinding BindInstance<T1, T2> (object instance, string name = null, bool makeDefault = false) 
			where T1:class where T2:class 
		{
			return Bind(InstantiationType.Instance, new List<Type>{typeof(T1), typeof(T2)}, () => instance, name, makeDefault);
		}

		public IBinding BindInstance<T1, T2, T3> (object instance, string name = null, bool makeDefault = false) 
			where T1:class where T2:class where T3:class 
		{
			return Bind(InstantiationType.Instance, new List<Type>{typeof(T1), typeof(T2), typeof(T3)}, () => instance, name, makeDefault);
		}

		public IBinding BindInstance<T1, T2, T3, T4> (object instance, string name = null, bool makeDefault = false) 
			where T1:class where T2:class where T3:class where T4:class 
		{
			return Bind(InstantiationType.Instance, new List<Type>{typeof(T1), typeof(T2), typeof(T3), typeof(T4)}, () => instance, name, makeDefault);
		}

		public IBinding BindInstance<T1, T2, T3, T4, T5> (object instance, string name = null, bool makeDefault = false) 
			where T1:class where T2:class where T3:class where T4:class where T5:class
		{
			return Bind(InstantiationType.Instance, new List<Type>{typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)}, () => instance, name, makeDefault);
		}
	}
}
