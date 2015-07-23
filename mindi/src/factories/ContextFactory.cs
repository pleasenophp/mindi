using System;
using System.Collections;
using minioc;

using minioc.context.bindings;
using minioc.resolution.instantiator;
using MinDI.Context;
using MinDI.StateObjects;


namespace MinDI.Factories {
	/// <summary>
	/// Standard factory to resolve an object from context
	/// </summary>
	public class ContextFactory<T> : OpenContextObject, IDIFactory<T> where T:class
	{
		protected ContextEnvironment environment;

		protected override void OnContextInjected() {
			environment = context.Resolve<ContextEnvironment>();
		}

		public virtual T Resolve (string name = null) {
			// In the remote objects environment, chaining context, and creating RemoteObjectsDescriptor
			if (environment == ContextEnvironment.RemoteObjects) {
				IDIContext newContext = ContextHelper.CreateContext(this.context);
				BindObjectsRecord(newContext);
				return Resolve(newContext, name);
			}
			else {
				return Resolve(context, name);
			}
		}

		public T Destroy(T instance) {
			if (instance == null) {
				return null;
			}

			if (environment != ContextEnvironment.RemoteObjects) {
				return null;
			}

			// Getting context of the object
			IDIClosedContext contextObject = instance as IDIClosedContext;
			if (contextObject == null) {
				throw new MindiException(
					string.Format("Called Destroy on object {0} that doesn't implement IDIClosedContext. Consider deriving the object from the ContextObject",
						instance));
			}

			if (contextObject.context == null) {
				throw new MindiException(string.Format("Context is null for object {0}", instance));
			}

			IRemoteObjectsRecord ror = contextObject.context.Resolve<IRemoteObjectsRecord>();
			ror.DestroyAll();
			return null;

			// When we resolve an object on the new context and this object is mono behaviour that is instantiated:
			// - the object records its creation on the RemoteObjectsDescriptor in the context
			// (that is always present in the factory context)
			// If an object is created by a child factory, this factory records in our object descriptor 
			// the pair - factory - instance

			// When we destroy the object from the factory, and this object is recoreded as root in RemoteObjectsDescriptor
			// Then we just clear all the remote objects by calling GameObject.Destroy on them 

			// If we meet a child factory object - then we call destroy on child factory
		}


		protected T Resolve(IDIContext context, string name) {
			if (string.IsNullOrEmpty(name)) {
				return context.Resolve<T>();
			}
			else {
				return context.Resolve<T>(name);
			}
		}

		protected void BindObjectsRecord(IDIContext context) {
			var bind = context.CreateBindHelper();
			bind.singleton.Bind<IRemoteObjectsRecord>(() => new RemoteObjectsRecord());
		}
	}
}
