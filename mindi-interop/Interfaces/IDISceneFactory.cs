using System;
using System.Collections;

namespace MinDI {

	/// <summary>
	/// A factory interface to produce objects
	/// </summary>
	public interface IDISceneFactory : IDestroyingFactory
	{
		T Create <T>(string sceneName, string bindingName = null) where T:class, ISceneObject;
	}

}