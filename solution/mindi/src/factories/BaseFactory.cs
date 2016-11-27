using System;
using System.Collections;
using minioc;


using MinDI.Context;
using MinDI.StateObjects;
using MinDI.Introspection;


namespace MinDI.Factories {
	/// <summary>
	/// General factory class, don't derive from this object
	/// </summary>
	public abstract class BaseFactory<T> : OpenContextObject, IDestroyingFactory
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

			IDIClosedContext contextObject = instance as IDIClosedContext;
			if (contextObject != null && contextObject.IsValid()) {
				contextObject.BeforeFactoryDestruction();
			}

			if (environment != ContextEnvironment.RemoteObjects) {
				return;
			}
				
			VerifyObjectDestruction(instance);
			DestroyRemoteObjects(instance);
			RegisterDestruction(instance);
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

		protected void VerifyObjectCreation (string name, object instance, IDIContext resolutionContext) {
			IDIClosedContext contextObject = instance as IDIClosedContext;
			if (contextObject == null || !contextObject.IsValid()) {
				IBinding desc = resolutionContext.Introspect<T>(name);
				VerifyInstantiationContext(desc, resolutionContext, instance);
				return;
			}

			VerifyInstantiationContext(contextObject.descriptor.bindingDescriptor, resolutionContext, instance);
			if (contextObject.descriptor.factory != null) {
				throw new MindiException("Attempting to create an already created object on factory: "+this+". Object: "+instance+". If the object is bound as singleton, you cannot create it on the factory again !");
			}

			contextObject.descriptor.factory = this;
		}

		protected void VerifyInstantiationContext(IBinding desc, IDIContext resolutionContext, object instance) {
			if (desc != null && desc.instantiationType == InstantiationType.Concrete && desc.context != resolutionContext) {
				throw new MindiException(string.Format("Cannot instantiate an object {0} on factory {1}, because it is already a singleton on different context, than the one, this factory resolves objects on. " +
					"Consider making object multiple, or rebind it as singletone on this factory chaining context.", instance, this));
			}
		}

		protected void VerifyObjectDestruction(object instance) {
			IDIClosedContext contextObject = instance as IDIClosedContext;
			if (contextObject == null || !contextObject.IsValid()) {
				return;
			}

			if (contextObject.descriptor.factory != this) {
				throw new MindiException(string.Format("The object {0} has not been created on this factory: {1}", contextObject, this));
			}

			if (contextObject.descriptor.context == null) {
				throw new MindiException(string.Format("Context is null for object {0}", contextObject));
			}

		}

		protected void DestroyRemoteObjects(object instance) {
			IDIClosedContext contextObject = instance as IDIClosedContext;
			if (contextObject != null && contextObject.IsValid()) {
				IRemoteObjectsRecord ror = contextObject.descriptor.context.Resolve<IRemoteObjectsRecord>();
				ror.DestroyAll();
			}
			else {
				IRemoteObjectsDestroyer rod = this.context.Resolve<IRemoteObjectsDestroyer>();
				IRemoteObjectsHash hash = this.context.Resolve<IRemoteObjectsHash>();
				rod.Destroy(instance, hash);
			}
		}

	}
}
