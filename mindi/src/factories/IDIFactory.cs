using System;
using System.Collections;
using minioc;

using minioc.context.bindings;
using minioc.resolution.instantiator;

namespace MinDI.Factories {

	/// <summary>
	/// A factory interface to produce objects
	/// </summary>
	public interface IDIFactory<T> : IDestroyingFactory where T:class
	{
		T Create (string name = null);
		T Destroy(T instance);
	}

}