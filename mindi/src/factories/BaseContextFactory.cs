using System;
using System.Collections;
using minioc;

using minioc.context.bindings;
using minioc.resolution.instantiator;
using MinDI.Context;
using MinDI.StateObjects;
using MinDI.Introspection;
using MinDI.Factories;
using System.Collections.Generic;
using MinDI.Resolution;


namespace MinDI {
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
			IDIContext newContext = this.context;
			if (environment == ContextEnvironment.RemoteObjects || ForceNewContext()) {
				newContext = newContext.Reproduce();
				InitNewContext(newContext);
			}

			if (customContextInitializer != null) {
				customContextInitializer(newContext);
			}
				
			// In the remote objects environment, chaining context, and creating RemoteObjectsDescriptor
			if (environment == ContextEnvironment.RemoteObjects) {
				BindObjectsRecord(newContext);
			}

			T instance = CreateObjectInternal(newContext, bindingName, construction);
			return instance;
		}
			
		private T CreateObjectInternal(IDIContext context, string name, Func<IConstruction> construction) {
			T instance;
			if (!SoftCreation()) {
				instance = context.Resolve<T>(construction, name);
			}
			else {
				instance = context.TryResolve<T>(construction, name);
			}

			if (instance != null) {
			 	VerifyObjectCreation(name, instance, context);

				if (environment == ContextEnvironment.RemoteObjects) {
					RegisterCreation(instance);
				}
			}

			return instance;
		}
	}
}
