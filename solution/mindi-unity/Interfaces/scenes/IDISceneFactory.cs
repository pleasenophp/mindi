using System;
using MinDI.Resolution;

namespace MinDI.Unity {
	/// <summary>
	/// A factory interface to produce objects
	/// </summary>
	public interface IDISceneFactory : IDestroyingFactory {
		T Create<T>(string sceneName, bool destroyableObjects, string bindingName = null,
			Action<IDIContext> customContextInitializer = null, Func<IConstruction> construction = null)
			where T : class, ISceneObject;
	}
}