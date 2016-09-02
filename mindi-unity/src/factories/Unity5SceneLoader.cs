﻿using UnityEngine;
using System.Collections;
using MinDI;
using System;

namespace MinDI.Unity {

	public class Unity5SceneLoader : SceneLoader
	{
		private interface ILoading
		{
			string loadingLevelName { get; }
			ISceneObject Load (IDISceneFactory factory);
		}

		private class LoadingClass<T> : ILoading where T : class, ISceneObject
		{
			public string loadingLevelName { get; set; }
			public Action<T> callback { get; set; }
			public ISceneArguments arguments { get; set; }

			public LoadingClass (string name, ISceneArguments arguments, Action<T> callback)
			{
				this.loadingLevelName = name;
				this.callback = callback;
				this.arguments = arguments;
			}

			public ISceneObject Load (IDISceneFactory factory)
			{
				T instance = null;
				if (arguments == null) {
					instance = factory.Create<T> (Application.loadedLevelName, true);
				} else {
					instance = factory.Create<T> (Application.loadedLevelName, true, null, arguments.PopulateContext, arguments.CreateConstruction);
				}

				if (callback != null) {
					callback (instance);
				}
				return instance;
			}

		}

		[Injection]
		public IDISceneFactory sceneFactory { get; set; }

		private ISceneObject currentScene = null;
		private ILoading loading = null;

		public override void Load<T> (string name, ISceneArguments arguments = null, Action<T> callback = null)
		{
			if (currentScene != null) {
				sceneFactory.DestroyInstance (currentScene);
				currentScene = null;
			}

			loading = new LoadingClass<T> (name, arguments, callback);

			// StartCoroutine (LoadSceneCoroutine (name)); // This unity freezing error should be fixed by their team - else cgange it back to coroutine with yelding 1 frame at teh start
			LoadScene(name);
		}

		private void LoadScene(string sceneName)
		{
			Application.LoadLevel (sceneName);
			if (!Application.isLoadingLevel) {
				loading = null;
				throw new MindiException ("The scene not found with name: " + sceneName);
			}
		}

		void OnLevelWasLoaded (int level)
		{
			string loadedLevelName = Application.loadedLevelName.ToLower ();

			if (loadedLevelName == ApplicationStarter.RootSceneName.ToLower ()) {
				return;
			}

			if (loading == null || loading.loadingLevelName.ToLower () != loadedLevelName) {
				Debug.LogError ("The level " + Application.loadedLevelName + " was loaded incorrectly. Please use ISceneLoader to load the level with MinDI.");

				if (loading != null) {
					Debug.LogWarning ("Loading level was set to: " + loading.loadingLevelName);
				}

			}

			if (loading != null) {
				currentScene = loading.Load (this.sceneFactory);
				loading = null;
			}
		}
	}
}
