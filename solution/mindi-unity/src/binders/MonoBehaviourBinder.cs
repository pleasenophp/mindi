using System;
using System.Collections;
using MinDI.StateObjects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MinDI.Binders {

	public class MonoBehaviourBinder : OpenContextObject {
		private readonly BaseDIBinder baseBinder;

		private IRemoteObjectsHash _objectsHash;
		private IRemoteObjectsHash objectsHash {
			get {
				return _objectsHash ?? (_objectsHash = this.context.Resolve<IRemoteObjectsHash>());
			}
		}

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
		}
			
		public IBinding Bind<T, TInstance> (string name = null) 
			where T:class where TInstance:MonoBehaviour, T

		{
			return baseBinder.Bind<T>(() => this.Resolve<T, TInstance>(null), name);
		}

		public IBinding Bind<TInstance> (string name = null) where TInstance:MonoBehaviour {
			return Bind<TInstance, TInstance>(name);
		}

		public IBinding BindPrefab<T, TInstance> (Func<GameObject> prefabLocator, string bindingName = null) 
			where T:class where TInstance:MonoBehaviour, T

		{
			return baseBinder.Bind<T>(() => ResolvePrefab<T, TInstance>(prefabLocator), bindingName);
		}

		public IBinding BindPrefab<TInstance> (Func<GameObject> prefabLocator, string bindingName = null) 
			where TInstance:MonoBehaviour 
		{
			return BindPrefab<TInstance, TInstance>(prefabLocator, bindingName);
		}

		public IBinding BindResource<T, TInstance> (string resourceName, string bindingName = null) 
			where T:class where TInstance:MonoBehaviour, T

		{
			return baseBinder.Bind<T>(() => ResolveResource<T, TInstance>(resourceName), bindingName);
		}

		public IBinding BindResource<TInstance> (string resourceName, string bindingName = null) 
			where TInstance:MonoBehaviour 
		{
			return BindResource<TInstance, TInstance>(resourceName, bindingName);
		}
			
		private T Resolve<T, TInstance> (Func<GameObject> objectLocator) 
			where T:class where TInstance:MonoBehaviour, T
		{
			GameObject obj = null;
			if (objectLocator != null) {
				obj = objectLocator();
				if (obj == null) {
					throw new MindiException(string.Format("Binding for {0}->{1}: cannot locate the game object to put the MonoBehaviour on", 
						typeof(T), typeof(TInstance)));
				}
			}

			if (obj == null) {
				string objectName = typeof(TInstance).Name;
				obj = new GameObject(objectName);
				objectsHash.Register(obj);
			}
			else {
				objectsHash.Register(obj);
			}
				
			return obj.AddComponent<TInstance>();
		}

	
		private T ResolveResource<T, TInstance>(string path) 
				where T:class where TInstance:MonoBehaviour, T
		{
			var prefab = Resources.Load<GameObject>(path);

			if (prefab == null) {
				throw new MindiException("Cannot load the resource by path "+path);
			}
			return ResolveFromPrefab<T, TInstance>(prefab);
		}

		private T ResolvePrefab<T, TInstance>(Func<GameObject> prefabLocator) 
			where T:class where TInstance:MonoBehaviour, T
		{
			GameObject prefab = prefabLocator();
			if (prefab == null) {
				throw new MindiException(string.Format("PrefabBinding for {0}->{1}: the prefab was null", 
					typeof(T), typeof(TInstance)));
			}

			return ResolveFromPrefab<T, TInstance>(prefab);
		}

		private T ResolveFromPrefab<T, TInstance>(GameObject prefab) 
			where T:class where TInstance:MonoBehaviour, T
		{
			// This is hack for UnityEditor mode only - to not bind loaded prefabs to the scene
			objectsHash.Register(prefab);

			GameObject obj = (GameObject)Object.Instantiate(prefab);
			obj.name = typeof(TInstance).Name;

			objectsHash.Register(obj);
			var instance = obj.GetComponent<TInstance>();

			if (instance == null) {
				throw new MindiException(string.Format("No MonoBehaviour of type {0} found on prefab {1}",
					typeof(TInstance), prefab.name));
			}

			return instance;
		}

	}

}
