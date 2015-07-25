using System;
using System.Collections.Generic;
using MinDI.Introspection;

namespace MinDI {
	public interface IDIContext {
		void Register (IBinding binding);
		T Resolve<T> (bool omitInjectDependencies = false);
		T Resolve<T> (string name, bool omitInjectDependencies = false);
		BindingDescriptor Introspect<T>(string name=null);
		BindingDescriptor Introspect(Type type, string name=null);
		object Resolve (Type type, string name=null, bool omitInjectDependencies = false);
		void InjectDependencies (object instance, IList<IDependency> dependencies = null);
		void RemoveBinding (IBinding binding);
		IDIContext parent {get;}
		string name { get;}
	}
}
