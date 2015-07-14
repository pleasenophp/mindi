using System;
using System.Collections;
using minioc;

using minioc.context.bindings;
using minioc.resolution.instantiator;

namespace MinDI.Factories {

	/// <summary>
	/// A factory interface to produce objects
	/// </summary>
	public interface IDIFactory<T> where T:class
	{
		T Resolve (string name = null);
	}

}