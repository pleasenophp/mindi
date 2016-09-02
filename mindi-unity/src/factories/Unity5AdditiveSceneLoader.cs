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
	public class Unity5AdditiveSceneLoader : AdditiveSceneLoader {
		
		protected override T UnloadAdditive<T> (T obj)
		{
			this.sceneFactory.DestroyInstance (obj);
			return default (T);
		}

		protected override IEnumerator LoadAdditiveCoroutine<T> (string name, ISceneArguments arguments, Action<T> callback)
		{
			AsyncOperation async = Application.LoadLevelAdditiveAsync (name);
			yield return async;

			T sceneObject = null;
			if (arguments == null) {
				sceneObject = this.sceneFactory.Create<T> (name, false);
			} else {
				sceneObject = this.sceneFactory.Create<T> (name, false, null, arguments.PopulateContext, arguments.CreateConstruction);
			}

			callback (sceneObject);
		}
	}

}
