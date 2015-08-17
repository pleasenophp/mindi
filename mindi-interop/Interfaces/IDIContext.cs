using System;
using System.Collections.Generic;
using MinDI.Introspection;
using MinDI.Resolution;

namespace MinDI {
	public interface IDIContext {
		void Register (IBinding binding);

		T Resolve<T> (string name = null);
		T Resolve<T> (IConstruction construction, string name = null);
		object Resolve (Type type, string name=null);
		object Resolve (Type type, IConstruction construction);
		object Resolve (Type type, IConstruction construction, string name);

		void InjectDependencies (object instance, IConstruction construction = null);

		BindingDescriptor Introspect<T>(string name=null);
		BindingDescriptor Introspect(Type type, string name=null);
	
		void RemoveBinding (IBinding binding);
		IDIContext parent {get;}
		string name { get;}
	}
}
