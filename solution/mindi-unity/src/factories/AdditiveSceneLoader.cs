using System.Collections;
using System;


namespace MinDI.Unity {
	/*
	 *  ISceneLoader -> SceneLoader: is singletone on the root context and contains an IDISceneFactory
	 * This manager is used to load the scenes. The factory creates the scene context and sets the destroyable mono behaviour. 
	 * 
	 * IAdditiveSceneLoader -> AdditiveSceneLoader: is a multiple on the root context and can be instantiated on any level. Contains an IDISceneFactory
	 * This factory doesnt set the destroyable mono behaviour, but reads it from the root context. This can be used to create additive scene instances on any level.
	 * 
	 * IDISceneFactory itself is a multiple on the root context. ISceneObject is multiple on the root context.
	 * 
	 */
	public abstract class AdditiveSceneLoader : ContextObject, IAdditiveSceneLoader {
		[Injection] public IDISceneFactory sceneFactory { get; set; }

		[Injection] public ICoroutineManager coroutines { get; set; }


		public void Load(string name, Action<ISceneObject> callback) {
			Load<ISceneObject>(name, null, callback);
		}

		public void Load(string name, ISceneArguments arguments, Action<ISceneObject> callback) {
			Load<ISceneObject>(name, arguments, callback);
		}

		public void Load<T>(string name, Action<T> callback) where T : class, ISceneObject {
			Load<T>(name, null, callback);
		}

		public void Load<T>(string name, ISceneArguments arguments, Action<T> callback) where T : class, ISceneObject {
			coroutines.StartCoroutine(LoadAdditiveCoroutine<T>(name, arguments, callback));
		}


		public ISceneObject Unload(ISceneObject obj) {
			return Unload<ISceneObject>(obj);
		}

		public T Unload<T>(T obj) where T : class, ISceneObject {
			return UnloadAdditive(obj);
		}

		protected abstract T UnloadAdditive<T>(T obj) where T : class, ISceneObject;
		protected abstract IEnumerator LoadAdditiveCoroutine<T>(string name, ISceneArguments arguments, Action<T> callback) where T : class, ISceneObject;
	}
}