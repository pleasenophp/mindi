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
 * v 4.7) Make the callback for non-additive scene work
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
 * v 9.6) Allow creation and destruction of non-context objects on factories, with some limitations
 * 
 * v 9.7) Create derived classes for unity, so objects can be copied
 * 
 * v 9.8) No exception bug when something is not resolved
 * 
 * 9.8.1) Check why the not context monobehaviour is not spawning from resources when injected
 * 
 * 
 * 9.9) Add a way to pass the manual dependencies to the object (construction dependencies).
 * 		[Injection] - tryes to inject the property from context
 * 		[SoftInjection] - tryes to inject the property from context, but doesn't throw exception if it cannot
 * 		[Requirement] - tryes to inject the property from the construction expression
 * 		[SoftRequirement] - tries to inject the property from the construction expression, but doesn't throw exception if it cannot
 * 		[RequirementInjection] - first try to inject from construction, then from injection context
 * 		[SoftRequirementInjection] - the same as above, without exception
 * 
 * 		+ base fields injection with type reassigning can be usefull. 
 * 			[InjectBaseField<BaseViewHandler>((s) => s.vistraEvent]
 *			public PlaceOperation placeOperation { get; }
 * 
 * 		+ See how these things work on methods injection - maybe it's good to leave injection methods here
 * 
 * 
 * 9.9.1) Check when an object has at least one Injection attribute, but is not IDIClosedContext - throw exception
 * 
 * 9.9.2) Make the scene loader accept the custom parameters
 * 
 * 9.9.9) Version number to DLLs and other release prepare things - going to beta stage
 *  
 * 10) Make internal documentation and object diagrams
 * 
 * 10.5) Sort out the lifetime control when resolving the mono behaviours to existing objects
 * 
 * 10.6) Make the binding to existing MB not allowed to resolve on factory. Also see if we can add additional control for the construction dependencies for the same thing
 * 
 * 11) Make programmer documentation and TODOs for the further guides
 * 
 * 12) Make singleton sorted in Minioc. For the generic binding allow to use multiple binding as well.
 * 
 * 13) Add better code to coroutine manager
 * 
 * 14) Add some editor helper scripts
 * 
 * 
 * 
 * Features to add later:
 * 
 * - write more tests for factories
 * 
 * - Serialization feature
 * 
 * - See how to easily see context of each providing library (context initializers observaton)
 * 
 * - Context profiler feature
 * 
 * - Maybe - IDE built in resolution lookup
 * 
 * 
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

