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
	public class ReproduceContextFactory<T, TInitializer> : ContextFactory<T>, IDIRFactory<T, TInitializer> 
	where T:class where TInitializer:IContextInitializer
	{
		protected override bool ForceNewContext() {
			return true;
		}

		protected override void InitNewContext(IDIContext context) {
			context.Initialize<TInitializer>();
		}
	}

}