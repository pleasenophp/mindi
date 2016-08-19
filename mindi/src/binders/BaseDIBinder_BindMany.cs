using System;
using System.Collections;
using minioc;
using minioc.context.bindings;
using MinDI.Introspection;
using System.Collections.Generic;

namespace MinDI.Binders {
	
	public abstract partial class BaseDIBinder
	{		
		public void BindMany<T1, T2> (Func<object> create, string name = null, bool makeDefault = false) 
		where T1:class where T2:class
		{
			Bind(this.instantiationType, new List<Type>{typeof(T1), typeof(T2)}, create, name, makeDefault);
		}


		public void BindMany<T1, T2, T3> (Func<object> create, string name = null, bool makeDefault = false) 
		where T1:class where T2:class where T3:class
		{
			Bind(this.instantiationType, new List<Type>{typeof(T1), typeof(T2), typeof(T3)}, create, name, makeDefault);
		}

		public void BindMany<T1, T2, T3, T4> (Func<object> create, string name = null, bool makeDefault = false)
		where T1:class where T2:class where T3:class where T4:class
		{
			Bind(this.instantiationType, new List<Type>{typeof(T1), typeof(T2), typeof(T3), typeof(T4)}, create, name, makeDefault);
		}

		public void BindMany<T1, T2, T3, T4, T5> (Func<object> create, string name = null, bool makeDefault = false)
		where T1:class where T2:class where T3:class where T4:class where T5:class
		{
			Bind(this.instantiationType, new List<Type>{typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)}, create, name, makeDefault);
		}


		public IBinding BindGenericMany (IList<Type> interfaceTypes, Type resolutionType, string name = null, bool makeDefault = false)
		{
			foreach (Type tp in interfaceTypes) {
				ValidateGenericType(tp);
			}
			ValidateGenericType(resolutionType);

			IBinding binding = CreateGenericBinding (interfaceTypes, resolutionType, name, makeDefault);
			context.Register (binding);
			return binding;
		}
	}
}
