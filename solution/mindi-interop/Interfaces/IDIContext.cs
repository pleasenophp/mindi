using System;
using System.Collections.Generic;
using MinDI.Introspection;
using MinDI.Resolution;

namespace MinDI {
	public interface IDIContext {
		void Register (IBinding binding);

		T Resolve<T> (string name = null);
		T Resolve<T> (Func<IConstruction> construction, string name = null);

		T TryResolve<T>(string name = null);
		T TryResolve<T>(Func<IConstruction> construction, string name = null);

		object TryResolve(Type type, string name = null);
		object TryResolve(Type type, Func<IConstruction> construction, string name = null);

		object Resolve (Type type, string name=null);
		object Resolve (Type type, Func<IConstruction> construction);
		object Resolve (Type type, Func<IConstruction> construction, string name);

		void InjectDependencies (object instance, Func<IConstruction> construction = null);

		IBinding Introspect<T>(string name=null);
		IBinding Introspect(Type type, string name=null);
	
		void RemoveBinding (IBinding binding);
		IDIContext parent {get;}
		string name { get;}
	}
}
