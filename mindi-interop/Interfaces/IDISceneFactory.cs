using System;
using System.Collections;

namespace MinDI {

	/// <summary>
	/// A factory interface to produce objects
	/// </summary>
	public interface IDISceneFactory<T> : IDestroyingFactory where T:ISceneObject
	{
		T Create (string sceneName, string bindingName = null);
		T Destroy(T instance);
	}

}