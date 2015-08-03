using UnityEngine;
using System.Collections;
using MinDI.Context;
using MinDI.StateObjects;
using MinDI.Unity;

namespace MinDI
{
	public class ApplicationStarter : ContextMonoBehaviour
	{
		public const string RootSceneName = "_root";
		public const string StarterObjectName = "_starter";

		public StarterBehaviour behaviour = StarterBehaviour.BackToThisScene;
		public string autoStartScene;

		[Injection]
		public IActionQueue queue {get; set;}

		[Injection]
		public ISceneLoader sceneLoader {get; set;}


		private UnityContextStart init;

		private bool ready = false;


		// The very first awake in the application
		void Awake ()
		{
			this.gameObject.name = StarterObjectName;
			GameObject existingStarter = GameObject.Find(StarterObjectName);

			if (existingStarter != null && existingStarter != this.gameObject) {
				Destroy(this.gameObject);
				return;
			}
		
			if (RootContainer.context != null) {
				return;
			}

			if (Application.loadedLevelName != RootSceneName) {
				LoadRootScene();
				return;
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

		private void LoadRootScene() {
			RootContainer.loadingRoot = true;

			if (this.behaviour == StarterBehaviour.BackToThisScene) {
				RootContainer.overrideAutoStartScene = Application.loadedLevelName;
			}
			else if (this.behaviour == StarterBehaviour.RootScene) {
				RootContainer.overrideAutoStartScene = null;
			}

			Application.LoadLevel(RootSceneName);
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

/*
 * Task list
 * 
 * v 1) Create nested factory and be able to destroy it all together
 * 
 * v 2) Make the additive scene creation on factory
 * 
 * v 2.4) Bind with a present mono behaviour on the scene
 * 
 * v 2.5) Make a callback after injection work on user level
 * 
 * v 3) Sort out the things with dont destroy on load
 * 
 * v 4) Sort out the things with auto object creations and separate application starter out. 
 * 
 * v 4.5) Make a non-additive moving to another scene work. 
 * 
 * v 4.6) Make the scene auto-load work
 * 
 * v 4.7) MaKe the callback for non-additive scene work
 * 
 * v 4.8) Additive scene factory should obey the general MB lifetime rule, so if it's created in permanent context, it also creates the things permanently
 * 
 * v 4.9) Move the related objects to the library.
 * 
 * v 4.10) Add missing bindings to MB binder.
 * 
 * v 5) Sort out the ContextObject. Make the common ContextObject with state available in minioc
 *   Maybe allow to inject dependencies only on context object, but the usual object can still be resolved (if it doesn't have any dependencies).
 * 
 * v 6) Make rebinding work through factories. Get rid of stContext object and find it instead
 * 
 * v 7) Solve the singleton subjectivisation problem
 * 
 * v 7.5) Sort out context monobehaviours requirement
 * 
 * v 8) Sort out other TODOs
 * 
 * v 9) Default-resolution of the generic factories (maybe the whole new feature - binding generics with default resolution)
 * 
 * v 9.5) Make the generic factories bindable on singletone (maybe rework singletone into minioc)
 * 
 * 9.6) Allow creation and destruction of non-context objects on factories, with some limitations
 * 
 * 10) Make internal documentation and object diagrams
 * 
 * 10.5) Sort out the lifetime control when resolving the mono behaviours to existing objects
 * 
 * 11) Make programmer documentation and TODOs for the further guides
 * 
 * 12) Make singleton sorted in Minioc
 * 
 * 13) Add better code to coroutine manager
 * 
 * 14) Add some editor helper scripts
 * 
 * 15) Sort the version numbers and other release prepare things
 * 
 * 
 * Features to add later:
 * 
 * 1) Serialization feature
 * 
 * 2) Context profiler feature
 * 
 * 3) Maybe - IDE built in resolution lookup
 * 
 * 
 * 
 *
 * 
 * 
 * 
 * NOTES
 * INTROSPECTION feature:
 * - Knowing the type of the object interface, we can get the context that has the resolver for this binding.
 * - We can also find the type of binding, that exists for this context. There is also possible to find a factory type if we somehow communicate this information.
 * - If this is done, then there is no need to keep the singleton context state on the object itself
 * 
 * - Context that starts in Unity is marked as UnityRoot. It means that it can create MonoBehaviours and they will become permanent
 * - Context that starts in the SceneFactory is marked as UnityUser. It can create mono behaviours but they are not understroyable
 * 
 * 
 * 

*/

