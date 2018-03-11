using UnityEngine;
using MinDI.StateObjects;
using MinDI.Unity;

namespace MinDI {
	public class ApplicationStarter : ContextMonoBehaviour {
		public const string RootSceneName = "_root";
		public const string StarterObjectName = "_starter";

		public string autoStartScene;

		[Injection] public IActionQueue queue { get; set; }

		[Injection] public ISceneLoader sceneLoader { get; set; }

		private UnityContextStart init;

		private bool ready = false;

		void Awake() {
			this.gameObject.name = StarterObjectName;
			GameObject existingStarter = GameObject.Find(StarterObjectName);

			if (existingStarter != null && existingStarter != this.gameObject) {
				Destroy(this.gameObject);
				return;
			}

			// The case where we loading directly from root scene
			DoInitialization();
		}

		private void DoInitialization() {
			// Ignoring starter on non-root scene
			if (Application.loadedLevelName != RootSceneName) {
				if (RootContainer.context == null) {
					throw new MindiException("Don't use starter on non-root scene. Use _rootRedirect instead.");
				}

				return;
			}

			if (RootContainer.context != null) {
				throw new MindiException("The root scene is loaded when the context is already initialized !");
			}

			init = this.GetComponent<UnityContextStart>();
			if (init == null) {
				throw new MindiException("UnityContextStart or any inherited component not found on the _starter object !");
			}

			// Adding remote objects validator
			RemoteObjectsHelper.AddValidator(new UnityRemoteObjectsValidator());

			// Creating context
			RootContainer.context = init.CreateContext();

			// Resolving root scene factory
			RootSceneFactory factory = RootContainer.context.Resolve<RootSceneFactory>();
			factory.Create<ISceneObject>(RootSceneName, false);

			// Global start of application
			init.GlobalStart(RootContainer.context);

			// Loading auto start scene
			LoadAutoStartScene();

			ready = true;
		}

		private void LoadAutoStartScene() {
			if (!string.IsNullOrEmpty(RootContainer.overrideAutoStartScene)) {
				sceneLoader.Load(RootContainer.overrideAutoStartScene);
				RootContainer.overrideAutoStartScene = null;
			}
			else if (!string.IsNullOrEmpty(autoStartScene)) {
				sceneLoader.Load(autoStartScene);
			}
		}

		void FixedUpdate() {
			if (!ready) {
				return;
			}

			queue.Process();
		}
	}
}