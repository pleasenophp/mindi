using UnityEngine;
using System.Collections;
using MinDI;
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
	public class AdditiveSceneLoader : ContextObject, IAdditiveSceneLoader {

		[Injection]
		public IDISceneFactory sceneFactory {get; set;}

		[Injection] 
		public ICoroutineManager coroutines {get; set;}


		public void Load(string name, Action<ISceneObject> callback) {
			Load<ISceneObject>(name, callback);
		}

		public void Load<T>(string name, Action<T> callback) where T:class, ISceneObject {
			coroutines.StartCoroutine(LoadAdditiveCoroutine<T>(name, callback));
		}


		public ISceneObject Unload(ISceneObject obj) {
			return Unload<ISceneObject>(obj);
		}

		public T Unload<T>(T obj) where T:class, ISceneObject {
			this.sceneFactory.DestroyInstance(obj);
			return default(T);
		}
		
		private IEnumerator LoadAdditiveCoroutine<T>(string name, Action<T> callback) where T:class, ISceneObject {
			AsyncOperation async = Application.LoadLevelAdditiveAsync(name);
			yield return async;

			T sceneObject = this.sceneFactory.Create<T>(name, false);
			callback(sceneObject);
		}

	}

}
