using System;
using System.Collections;
using MinDI.Context;

namespace MinDI {

	/// <summary>
	/// A chain factory interface to produce objects. This factory will always create new context.
	/// </summary>
	public interface IDIRFactory<T, TInitializer> : IDIFactory<T>  
		where T:class where TInitializer:IContextInitializer
	{
	}

}