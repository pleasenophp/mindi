using System;
using System.Collections;
using minioc;

using minioc.context.bindings;
using minioc.resolution.instantiator;

namespace MinDI.Factories {

	/// <summary>
	/// A factory interface to produce and destroy objects
	/// Usefull for mono behaviours and other dynamic objects where the destroying process should be controlled manually
	/// </summary>
	public interface IDIDestroyingFactory<T> : IDIFactory<T> where T:class
	{
		void Destroy(T instance);
	}

}