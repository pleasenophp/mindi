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
	public abstract class BaseDIFactory<T> : OpenContextObject, IDestroyingFactory
		where T:class
	{
		protected ContextEnvironment environment;

		protected override void OnInjected() {
			environment = context.Resolve<ContextEnvironment>();
		}
			
		public T Destroy(T instance) {
			DestroyInstance(instance);
			return null;
		}
			
		public void DestroyInstance(object instance) {
			if (instance == null) {
				return;
			}

			if (environment != ContextEnvironment.RemoteObjects) {
				return;
			}

			// Getting context of the object
			IDIClosedContext contextObject = instance as IDIClosedContext;
			if (contextObject == null) {
				throw new MindiException(
					string.Format("Called Destroy on object {0} that doesn't implement IDIClosedContext. Consider deriving the object from the ContextObject",
						instance));
			}

			if (contextObject.factory != this) {
				throw new MindiException(string.Format("The object {0} has not been created on this factory: {1}", instance, this));
			}

			if (contextObject.context == null) {
				throw new MindiException(string.Format("Context is null for object {0}", instance));
			}

			IRemoteObjectsRecord ror = contextObject.context.Resolve<IRemoteObjectsRecord>();
			ror.DestroyAll();

			RegisterDestruction(instance);
		}
			
		protected T Create(IDIContext context, string name) {
			T instance = null;

			if (string.IsNullOrEmpty(name)) {
				instance = context.Resolve<T>();
			}
			else {
				instance = context.Resolve<T>(name);
			}

			IDIClosedContext contextObject = instance as IDIClosedContext;
			if (contextObject != null) {
				contextObject.factory = this;
			}

			return instance;
		}

		// Register creation of an instance made on this factory in the parent ROR
		protected void RegisterCreation(T instance) {
			IRemoteObjectsRecord ror = this.context.Resolve<IRemoteObjectsRecord>();
			ror.Register(new FactoryObjectRecord(this, instance));
		}

		// Register destruction of an instance made on this factory in the parent ROR
		protected void RegisterDestruction(object instance) {
			IRemoteObjectsRecord ror = this.context.Resolve<IRemoteObjectsRecord>();
			ror.DestroyByType<FactoryObjectRecord>((f) => f.instance == instance && f.factory == this);
		}

		protected void BindObjectsRecord(IDIContext context) {
			context.s().Rebind<IRemoteObjectsRecord>(null, "factory");
		}
	}
}
