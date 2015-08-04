using UnityEngine;
using System.Collections;
using MinDI;
using System;

namespace MinDI.Unity {

	public class SceneLoader : ContextMonoBehaviour, ISceneLoader {

		private interface ILoading {
			string loadingLevelName {get;}
			ISceneObject Load(IDISceneFactory factory);
		}

		private class LoadingClass<T> : ILoading where T:class, ISceneObject {
			public string loadingLevelName {get; set;}
			public Action<T> callback {get; set;}

			public LoadingClass(string name, Action<T> callback) {
				this.loadingLevelName = name;
				this.callback = callback;
			}

			public ISceneObject Load(IDISceneFactory factory) {
				T instance = factory.Create<T>(Application.loadedLevelName, true);
				if (callback != null) {
					callback(instance);
				}
				return instance;
			}

		}


		[Injection]
		public IDISceneFactory sceneFactory {get; set;}

		private ISceneObject currentScene = null;
		private ILoading loading = null;
		

		public void Load(string name, Action<ISceneObject> callback = null) {
			Load<ISceneObject>(name, callback);
		}

		public void Load<T>(string name, Action<T> callback = null) where T:class, ISceneObject {
			if (currentScene != null) {
				sceneFactory.DestroyInstance(currentScene);
				currentScene = null;
			}

			loading = new LoadingClass<T>(name, callback);

			Application.LoadLevel(name);
			if (!Application.isLoadingLevel) {
				loading = null;
				throw new MindiException("The scene not found with name: "+name);
			}
		}

		void OnLevelWasLoaded(int level) {
			// Debug.LogWarning("CALLED LEVEL WAS LOADED "+Application.loadedLevelName);

			if (Application.loadedLevelName == ApplicationStarter.RootSceneName) {
				if (RootContainer.loadingRoot) {
					RootContainer.loadingRoot = false;
					return;
				}
				else {
					throw new MindiException(string.Format("The {0} scene should never be loaded manually !", ApplicationStarter.RootSceneName));
				}
			}

			if (loading == null || loading.loadingLevelName != Application.loadedLevelName) {
				// TODO - fix it - gives sometimes a wrong message from the start scene

				Debug.LogError("The level "+Application.loadedLevelName+" was loaded incorrectly. Please use ISceneLoader to load the level with MinDI.");
			}

			if (loading != null) {
				currentScene = loading.Load(this.sceneFactory);
				loading = null;
			}
		}
		
	}
}
