using MinDI.Context;
using MinDI.StateObjects;
using MinDI.Introspection;


namespace MinDI.Factories.Internal {

	// TODO - we need to review how we create / destroy usual objects and remote objects
	// Currently the destructions is sometimes called 2-3 times. The problem is workarounded in the ContextObject, but it might be possible to avoid it in the factory level.


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

			// NOTE - this might be good to do only if it's not remote objects environment
			var contextObject = instance as IDIClosedContext;
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
			var ror = this.context.Resolve<IRemoteObjectsRecord>();
			ror.Register(new FactoryObjectRecord(this, instance));
		}

		// Register destruction of an instance made on this factory in the parent ROR
		protected void RegisterDestruction(object instance) {
			var ror = this.context.Resolve<IRemoteObjectsRecord>();
			ror.DestroyByType<FactoryObjectRecord>((f) => f.instance == instance && f.factory == this);
		}

		protected void BindObjectsRecord(IDIContext ctx) {
			ctx.s().Rebind<IRemoteObjectsRecord>(null, "factory");
		}

		protected void VerifyObjectCreation (string name, object instance, IDIContext resolutionContext) {
			var contextObject = instance as IDIClosedContext;
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
				throw new MindiException(string.Format("Cannot instantiate an object {0} on factory {1}, because it is already a singleton on different ctx, than the one, this factory resolves objects on. " +
					"Consider making object multiple, or rebind it as singletone on this factory chaining ctx.", instance, this));
			}
		}

		protected void VerifyObjectDestruction(object instance) {
			var contextObject = instance as IDIClosedContext;
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
			var contextObject = instance as IDIClosedContext;
			if (contextObject != null && contextObject.IsValid()) {
				var ror = contextObject.descriptor.context.Resolve<IRemoteObjectsRecord>();
				ror.DestroyAll();
			}
			else {
				var rod = this.context.Resolve<IRemoteObjectsDestroyer>();
				var hash = this.context.Resolve<IRemoteObjectsHash>();
				rod.Destroy(instance, hash);
			}
		}

	}
}
