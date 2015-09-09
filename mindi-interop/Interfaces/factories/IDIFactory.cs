using System;
using System.Collections;
using MinDI.Resolution;

namespace MinDI {

	/// <summary>
	/// A factory interface to produce objects
	/// </summary>
	public interface IDIFactory<T> : IDestroyingFactory where T:class
	{
		T Create (string name = null);
		T Create (Func<IConstruction> construction, string name = null);
		T Destroy(T instance);
	}

}