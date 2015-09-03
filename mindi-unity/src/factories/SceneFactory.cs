using UnityEngine;
using System.Collections;
using MinDI;
using MinDI.Context;
using MinDI.StateObjects;
using MinDI.Introspection;
using MinDI.UnityEditor;
using System.Collections.Generic;
using MinDI.Factories;
using System;
using MinDI.Resolution;

namespace MinDI.Unity {

	public class SceneFactory : BaseFactory<ISceneObject>, IDISceneFactory {

		[Injection]
		public IEditorPrefabFilter editorPrefabFilter { get; set; }
		
		public virtual T Create <T>(string sceneName, bool destroyableObjects, string bindingName = null, 
			Action<IDIContext> customContextInitializer = null, Func<IConstruction> construction = null) 
			where T:class, ISceneObject
		{
			if (environment != ContextEnvironment.RemoteObjects) {
				throw new MindiException("SceneFactory can only work in the Remote objects environment");
			}

			IDIContext newContext = ContextHelper.CreateContext (this.context);

			if (destroyableObjects) {
				newContext.s().BindInstance<MBLifeTime>(MBLifeTime.DestroyWithScene);
			}

			IList<ISceneContextInitializer> initializers = ContextBuilder.Initialize<ISceneContextInitializer>(newContext, new SceneContextAttribute(sceneName));
			BindObjectsRecord(newContext);

			if (customContextInitializer != null) {
				customContextInitializer(newContext);
			}

			return CreateScene<T>(newContext, initializers, sceneName, bindingName, construction);
		}
		
		protected T CreateScene<T> (IDIContext sceneContext, IList<ISceneContextInitializer> initializers, string sceneName, string bindingName = null, Func<IConstruction> construction = null) 
			where T:class, ISceneObject
		{
		
			// Injecting dependencies on the objects that should have dependencies, and tracking the other objects on ROR
			TrackObjects(sceneContext);

			// Adding auto-instantiated objects
			PerformAutoInstantiation(sceneContext, initializers);

			// Creating scene instance object
			SceneObject instance = sceneContext.Resolve<T>(construction, bindingName) as SceneObject;
			if (instance == null) {
				throw new MindiException("Scene object is expected to be inherited from SceneObject");
			}

			VerifyObjectCreation(bindingName, instance, sceneContext);

			instance.name = sceneName;
			RegisterCreation(instance);

			return instance as T;
		}

		private void TrackObjects(IDIContext sceneContext) {
			IRemoteObjectsHash hashObject = sceneContext.Resolve<IRemoteObjectsHash>();
			IRemoteObjectsRecord ror = sceneContext.Resolve<IRemoteObjectsRecord>();

			var objects = Resources.FindObjectsOfTypeAll(typeof(GameObject));
			foreach (GameObject obj in objects) {

				if (obj.hideFlags != HideFlags.None) {
					continue;
				}

				if (editorPrefabFilter.IsPrefab(obj)) {
					continue;
				}
					
				if (hashObject.hash.Contains(obj.GetInstanceID())) {
					continue;
				}
				hashObject.hash.Add(obj.GetInstanceID());

				// Find all the objects on the scene that have no hash, that's the new objects, and track them on this scene
				ror.Register(obj);

				// Injecting mono behaviours
				RegisterMonoBehaviours(obj, sceneContext, ror);
			}

		}

		private void RegisterMonoBehaviours(GameObject obj, IDIContext sceneContext, IRemoteObjectsRecord ror) {
			ContextMonoBehaviour[] behaviours = obj.GetComponents<ContextMonoBehaviour>();
			foreach (ContextMonoBehaviour mb in behaviours) {
				sceneContext.InjectDependencies(mb);
			}
		}

		private void PerformAutoInstantiation(IDIContext sceneContext, IList<ISceneContextInitializer> initializers) {
			foreach (ISceneContextInitializer initializer in initializers) {
				initializer.AutoInstantiate(sceneContext);
			}
		}
	}

}