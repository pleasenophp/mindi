using System;
using System.Collections;
using minioc;
using minioc.context.bindings;
using minioc.resolution.instantiator;
using UnityEngine;
using MinDI.StateObjects;

namespace MinDI.Binders {

	public class MonoBehaviourBinder : OpenContextObject {
		private IDIBinder baseBinder;
		private IRemoteObjectsHash objectsHash;

		public MonoBehaviourBinder(IDIContext context) : this (context, InstantiationMode.SINGLETON) {
		}

		protected MonoBehaviourBinder(IDIContext context, InstantiationMode mode) {
			this.contextInjection = context;

			var b = context.CreateBindHelper();

			if (mode == InstantiationMode.SINGLETON) {
				baseBinder = b.singleton;
			}
			else if (mode == InstantiationMode.MULTIPLE) {
				baseBinder = b.multiple;
			}

			objectsHash = context.Resolve<IRemoteObjectsHash>();
		}


		// TODO - add different binding types - for prefab, etc
		// TODO - add resolution of the ContextType, and bind depending on it as DontDestroyOnLoad
		// ContextType: Normal (not allowed to bind MB at all), UnityRoot (always create MB as DontDestroyOnLoad, 
		// Unity - Create MB normally

		public IBinding Bind<T, TInstance> (string name = null) 
			where T:class where TInstance:MonoBehaviour, T

		{
			return baseBinder.Bind<T>(() => this.Resolve<T, TInstance>(), name);
		}

		public IBinding BindResource<T, TInstance> (string resourceName, string bindingName = null) 
			where T:class where TInstance:MonoBehaviour, T

		{
			return baseBinder.Bind<T>(() => ResolveResource<T, TInstance>(resourceName), bindingName);
		}
			
		private T Resolve<T, TInstance> () 
			where T:class where TInstance:MonoBehaviour, T
		{
			string objectName = typeof(TInstance).Name;
			GameObject obj = new GameObject(objectName);
			BindInstantiation(obj, MBInstantiationType.NewObject);
			return obj.AddComponent<TInstance>();
		}

		private T ResolveResource<T, TInstance>(string path) 
				where T:class where TInstance:MonoBehaviour, T
		{
			GameObject prefab = Resources.Load<GameObject>(path);
			if (prefab == null) {
				throw new MindiException("Cannot load the resource by path "+path);
			}

			// This is hack for UnityEditor mode only - to not bind loaded prefabs to the scene
			objectsHash.hash.Add(prefab.GetInstanceID());

			GameObject obj = (GameObject)GameObject.Instantiate(prefab);
			obj.name = typeof(TInstance).Name;
			BindInstantiation(obj, MBInstantiationType.NewObject);
			return obj.GetComponent<TInstance>();
		}

		private void BindInstantiation(GameObject obj, MBInstantiationType instantiation) {
			DestroyBehaviour destroyBehaviour = obj.GetComponent<DestroyBehaviour>();
			if (destroyBehaviour != null) {
				destroyBehaviour.instantiationType = MBInstantiationType.ExistingObject;
				return;
			}

			destroyBehaviour = obj.AddComponent<DestroyBehaviour>();
			destroyBehaviour.instantiationType = instantiation;

			objectsHash.hash.Add(obj.GetInstanceID());
		}

	}

}
