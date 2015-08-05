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
	public abstract class BaseContextFactory<T> : BaseFactory<T>, IDestroyingFactory
		where T:class
	{

		// Override so the context is always reproduced
		// Will be always reproduced in the remote environment
		protected virtual bool ForceNewContext() {
			return false;
		}

		// Override to add some bindings to the new context
		protected virtual void InitNewContext(IDIContext context) {
		}

		// Override to init instance with custom data
		protected virtual void InitInstance(T instance) {
		}

		protected T CreateInstance(string bindingName = null) {
			IDIContext newContext = this.context;
			if (environment == ContextEnvironment.RemoteObjects || ForceNewContext()) {
				newContext = newContext.Reproduce();
				InitNewContext(newContext);
			}
				
			// In the remote objects environment, chaining context, and creating RemoteObjectsDescriptor
			if (environment == ContextEnvironment.RemoteObjects) {
				BindObjectsRecord(newContext);
			}

			T instance = CreateObjectInternal(newContext, bindingName);
			InitInstance(instance);
			return instance;
		}
			
		private T CreateObjectInternal(IDIContext context, string name) {
			T instance = context.Resolve<T>(name);
			VerifyObjectCreation(name, instance, context);

			if (environment == ContextEnvironment.RemoteObjects) {
				RegisterCreation(instance);
			}

			return instance;
		}
	}
}
