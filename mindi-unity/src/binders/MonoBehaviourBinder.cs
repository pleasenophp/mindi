using System;
using System.Collections;
using MinDI.StateObjects;
using UnityEngine;

namespace MinDI.Binders {

	public class MonoBehaviourBinder : OpenContextObject {
		private IDIBinder baseBinder;
		private IRemoteObjectsHash objectsHash;

		public MonoBehaviourBinder(IDIContext context) : this (context, InstantiationMode.SINGLETON) {
		}

		protected MonoBehaviourBinder(IDIContext context, InstantiationMode mode) {
			this.contextInjection = context;
		
			if (mode == InstantiationMode.SINGLETON) {
				baseBinder = context.s();
			}
			else if (mode == InstantiationMode.MULTIPLE) {
				baseBinder = context.m();
			}

			objectsHash = context.Resolve<IRemoteObjectsHash>();
		}


		// TODO - add different binding types - for prefab, etc

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
			TInstance instance = obj.GetComponent<TInstance>();

			if (instance == null) {
				throw new MindiException(String.Format("No MonoBehaviour of type {0} found on prefab {1}", 
					typeof(TInstance), prefab.name));
			}

			return instance;
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
