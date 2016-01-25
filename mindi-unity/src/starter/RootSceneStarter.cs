using UnityEngine;
using MinDI.Unity;

namespace MinDI
{
	public class RootSceneStarter : ContextMonoBehaviour
	{
		public bool backToThisScene = true;

		void Awake() {
			DoInitialization();
		}

		void DoInitialization() {
			if (Application.loadedLevelName != ApplicationStarter.RootSceneName) {
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
				RootContainer.overrideAutoStartScene = Application.loadedLevelName;
			}
			else {
				RootContainer.overrideAutoStartScene = null;
			}

			Application.LoadLevel(ApplicationStarter.RootSceneName);
		}
	}
}


