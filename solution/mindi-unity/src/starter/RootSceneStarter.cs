using UnityEngine;
using MinDI.Unity;
using System.Collections;
using UnityEngine.SceneManagement;

namespace MinDI
{
	public class RootSceneStarter : ContextMonoBehaviour
	{
		public bool backToThisScene = true;

		void Awake() {
			DoInitialization();
		}

		void DoInitialization() {
			if (SceneHelper.GetLoadedLevelName () != ApplicationStarter.RootSceneName) {
				LoadRootScene();
				return;
			}
			else {
				throw new MindiException("RootSceneStarter cannot be loaded on the root scene!");
			}
		}

		private void LoadRootScene() {
			// If the context is already initialized, don't load root scene again
			if (RootContainer.context != null) {
				return;
			}

			if (backToThisScene) {
				RootContainer.overrideAutoStartScene = SceneHelper.GetLoadedLevelName();
			}
			else {
				RootContainer.overrideAutoStartScene = null;
			}

			// TODO - trying without coroutine, as the bug in Unity must be fixed
			// StartCoroutine(LoadRootSceneCoroutine());
			SceneManager.LoadScene(ApplicationStarter.RootSceneName);
		}

		/*
		private IEnumerator LoadRootSceneCoroutine() {
			yield return 1;
			SceneManager.LoadScene(ApplicationStarter.RootSceneName);
		}
		*/
	}
}


