using System;
using System.Collections;
using minioc;

using minioc.context.bindings;
using minioc.resolution.instantiator;
using MinDI.Context;

namespace MinDI {

	/// <summary>
	/// A factory that chains context when creating a new object
	/// </summary>
	public class ContextChainFactory<T, TInitializer> : BaseDIFactory<T>, IDIChainFactory<T, TInitializer> 
	where T:class where TInitializer:IContextInitializer
	{
		public T Create (string name = null)
		{
			IDIContext newContext = ContextHelper.CreateContext (this.context);
			ContextBuilder.Initialize<TInitializer> (newContext);
			if (environment == ContextEnvironment.RemoteObjects) {
				BindObjectsRecord(newContext);
			}

			T instance = Create(newContext, name);

			return instance;
		}
	}

}