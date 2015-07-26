using System;
using System.Collections;
using minioc;

using minioc.context.bindings;
using minioc.resolution.instantiator;
using MinDI.Context;
using MinDI.StateObjects;
using MinDI.Introspection;


namespace MinDI {
	/// <summary>
	/// Standard factory to resolve an object from context
	/// </summary>
	public class ContextFactory<T> : BaseDIFactory<T>, IDIFactory<T>
		where T:class
	{
		public T Create (string name = null) {
			// In the remote objects environment, chaining context, and creating RemoteObjectsDescriptor
			if (environment == ContextEnvironment.RemoteObjects) {
				IDIContext newContext = ContextHelper.CreateContext(this.context);
				BindObjectsRecord(newContext);
				T instance = Create(newContext, name);
				RegisterCreation(instance);
				return instance;
			}
			else {
				return Create(context, name);
			}
		}

	}
}
