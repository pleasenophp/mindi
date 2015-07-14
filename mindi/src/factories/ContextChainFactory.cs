using System;
using System.Collections;
using minioc;

using minioc.context.bindings;
using minioc.resolution.instantiator;
using MinDI.Objects;
using MinDI.Context;

namespace MinDI.Factories {

	/// <summary>
	/// A factory that chains context when creating a new object
	/// </summary>
	public class ContextChainFactory<T, TInitializer> : PublicContextObject, IDIFactory<T> 
	where T:class where TInitializer:IContextInitializer
	{
		public T Resolve (string name = null)
		{
			IDIContext newContext = ContextHelper.CreateContext (this.context);
			ContextBuilder.Initialize<TInitializer> (newContext);

			if (string.IsNullOrEmpty(name)) {
				return newContext.Resolve<T> ();
			}
			else {
				return newContext.Resolve<T> (name);
			}
		}
	}

}