using UnityEngine;
using MinDI.Unity;

namespace MinDI {
	public class RootSceneStarter : ContextMonoBehaviour {
		public bool backToThisScene = true;

		void Awake() {
			DoInitialization();
		}

		private void DoInitialization() {
			if (Application.loadedLevelName != ApplicationStarter.RootSceneName) {
				LoadRootScene();
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
			RootContainer.overrideAutoStartScene = backToThisScene ? Application.loadedLevelName : null;
			Application.LoadLevel(ApplicationStarter.RootSceneName);
		}
	}
}