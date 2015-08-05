using System;
using System.Collections;
using minioc;

using minioc.context.bindings;
using minioc.resolution.instantiator;
using MinDI.Context;
using MinDI.StateObjects;
using MinDI.Introspection;


namespace MinDI.Factories {
	/// <summary>
	/// Standard factory to resolve an object from context
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
			if (contextObject == null) {
				BindingDescriptor desc = resolutionContext.Introspect<T>(name);
				VerifyInstantiationContext(desc, resolutionContext, instance);
				return;
			}

			VerifyInstantiationContext(contextObject.bindingDescriptor, resolutionContext, instance);
			if (contextObject.factory != null) {
				throw new MindiException("Attempting to create an already created object on factory: "+this+". Object: "+instance+". If the object is bound as singleton, you cannot create it on the factory again !");
			}

			contextObject.factory = this;
		}

		protected void VerifyInstantiationContext(BindingDescriptor desc, IDIContext resolutionContext, object instance) {
			if (desc.instantiationType == InstantiationType.Concrete && desc.context != resolutionContext) {
				throw new MindiException(string.Format("Cannot instantiate an object {0} on factory {1}, because it is already a singleton on different context, than the one, this factory resolves objects on. " +
					"Consider making object multiple, or rebind it as singletone on this factory chaining context.", instance, this));
			}
		}

		protected void VerifyObjectDestruction(object instance) {
			IDIClosedContext contextObject = instance as IDIClosedContext;
			if (contextObject == null) {
				return;
			}

			if (contextObject.factory != this) {
				throw new MindiException(string.Format("The object {0} has not been created on this factory: {1}", contextObject, this));
			}

			if (contextObject.context == null) {
				throw new MindiException(string.Format("Context is null for object {0}", contextObject));
			}

		}

		protected void DestroyRemoteObjects(object instance) {
			IDIClosedContext contextObject = instance as IDIClosedContext;
			if (contextObject != null) {
				IRemoteObjectsRecord ror = contextObject.context.Resolve<IRemoteObjectsRecord>();
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
