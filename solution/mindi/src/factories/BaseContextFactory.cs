using System;
using MinDI.Context;
using MinDI.Factories.Internal;
using MinDI.Resolution;

namespace MinDI {
	/// <summary>
	/// Abstract factory to implement custom factories, derive from this object
	/// </summary>
	public abstract class BaseContextFactory<T> : BaseFactory<T>
		where T:class
	{
		
		// Override so the context is always reproduced
		// Will be always reproduced in the remote environment
		protected virtual bool ForceNewContext() {
			return false;
		}

		// If set to true, the factory will use TryResolve and will return null if object is not bind in the context
		protected virtual bool SoftCreation() {
			return false;
		}

		// Override to add some bindings to the new context
		protected virtual void InitNewContext(IDIContext context) {
		}

		protected T CreateInstance(string name = null) {
			return CreateInstance(name, null, null);
		}

		protected T CreateInstance(Action<IDIContext> customContextInitializer) {
			return CreateInstance(null, customContextInitializer, null);
		}

		protected T CreateInstance(string name, Action<IDIContext> customContextInitializer) {
			return CreateInstance(name, customContextInitializer, null);
		}

		protected T CreateInstance(Func<IConstruction> construction) {
			return CreateInstance(null, null, construction);
		}

		protected T CreateInstance(string name, Func<IConstruction> construction) {
			return CreateInstance(name, null, construction);
		}

		protected T CreateInstance(Action<IDIContext> customContextInitializer, Func<IConstruction> construction) {
			return CreateInstance(null, customContextInitializer, construction);
		}
			
		protected T CreateInstance(string bindingName, Action<IDIContext> customContextInitializer, Func<IConstruction> construction) {
			var newContext = this.context;
			if (environment == ContextEnvironment.RemoteObjects || ForceNewContext()) {
				newContext = newContext.Reproduce();
				InitNewContext(newContext);

				if (customContextInitializer != null) {
					customContextInitializer(newContext);
				}
			}
			else if (customContextInitializer != null) {
				throw new MindiException("Custom context initializer is passed while the factory is not in the reproduce context mode: "+this);
			}
				
			// In the remote objects environment, chaining context, and creating RemoteObjectsDescriptor
			if (environment == ContextEnvironment.RemoteObjects) {
				BindObjectsRecord(newContext);
			}

			var instance = CreateObjectInternal(newContext, bindingName, construction);
			return instance;
		}
			
		private T CreateObjectInternal(IDIContext context, string name, Func<IConstruction> construction) {
			var instance = SoftCreation() ? context.TryResolve<T>(construction, name) : context.Resolve<T>(construction, name);
			if (instance == null) {
				return null;
			}
			VerifyObjectCreation(name, instance, context);
			if (environment == ContextEnvironment.RemoteObjects) {
				RegisterCreation(instance);
			}
			return instance;
		}
	}
}
