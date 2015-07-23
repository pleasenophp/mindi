using System;
using System.Collections;
using minioc;

using minioc.context.bindings;
using minioc.resolution.instantiator;
using MinDI.Objects;


namespace MinDI.Factories {
	/// <summary>
	/// Standard factory to resolve an object from context
	/// </summary>
	public class ContextFactory<T> : PublicContextObject, IDIFactory<T> where T:class
	{
		public T Resolve (string name = null) {
			if (string.IsNullOrEmpty(name)) {
				return context.Resolve<T>();
			}
			else {
				return context.Resolve<T>(name);
			}
		}

		public T Destroy(T instance) {
			if (instance == null) {
				return null;
			}

			// First of all need to force chaining context in factory if it's a Unity type of context
			// If the factory works on normal context, then it resolves in chaining mode only by request

			// When we resolve an object on the new context and this object is mono behaviour that is instantiated:
			// - the object records its creation on the RemoteObjectsDescriptor in the context
			// (that is always present in the factory context)
			// If an object is created by a child factory, this factory records in our object descriptor 
			// the pair - factory - instance

			// When we destroy the object from the factory, and this object is recoreded as root in RemoteObjectsDescriptor
			// Then we just clear all the remote objects by calling GameObject.Destroy on them 

			// If we meet a child factory object - then we call destroy on child factory

			throw new NotImplementedException();
		}
	}
}
