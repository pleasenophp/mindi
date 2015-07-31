using System;
using System.Collections;

namespace MinDI.Unity {

	/// <summary>
	/// A factory interface to produce objects
	/// </summary>
	public interface IDISceneFactory : IDestroyingFactory
	{
		T Create <T>(string sceneName, bool destroyableObjects, string bindingName = null) where T:class, ISceneObject;
	}

}