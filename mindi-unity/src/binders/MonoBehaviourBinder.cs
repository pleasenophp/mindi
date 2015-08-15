using System;
using System.Collections;
using MinDI.StateObjects;
using UnityEngine;

namespace MinDI.Binders {

	public class MonoBehaviourBinder : OpenContextObject {
		private BaseDIBinder baseBinder;
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
			
		public IBinding Bind<T, TInstance> (string name = null) 
			where T:class where TInstance:MonoBehaviour, T

		{
			return baseBinder.Bind<T>(() => this.Resolve<T, TInstance>(null), name);
		}

		public IBinding Bind<TInstance> (string name = null) where TInstance:MonoBehaviour {
			return Bind<TInstance, TInstance>(name);
		}
			
		// TODO - when binding to existing game object, need to check that the lifetime of this object
		// is greater or equal than the lifetime of our mono behaviour. Else - refuse to bind.
		// If the GO is in the ROR that is higher or equal than this ROR in the tree then it's ok
		// If the GO is in the ROR, we cannot find from this, so it's either in parralel ROR,
		// or in the lower ROR, so we cannot state that lifetime is the same or greater - not allowing

		public IBinding BindToGameObject<T, TInstance> (Func<GameObject> objectLocator, string name = null) 
			where T:class where TInstance:MonoBehaviour, T

		{
			return baseBinder.Bind<T>(() => this.Resolve<T, TInstance>(objectLocator), name);
		}

		public IBinding BindToGameObject<TInstance> (Func<GameObject> objectLocator, string name = null)
			where TInstance:MonoBehaviour
		{
			return BindToGameObject<TInstance, TInstance>(objectLocator, name);
		}

			
		public IBinding BindToExisting<T> (Func<GameObject> objectLocator, string name = null) 
			where T:class

		{
			return baseBinder.Bind<T>(() => this.ResolveExisting<T>(objectLocator), name);
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
				BindInstantiation(obj, MBInstantiationType.NewObject);
			}
			else {
				BindInstantiation(obj, MBInstantiationType.ExistingObject);
			}
				
			return obj.AddComponent<TInstance>();
		}

		private T ResolveExisting<T> (Func<GameObject> objectLocator) 
			where T:class
		{
			GameObject obj = objectLocator();
			if (obj == null) {
				throw new MindiException(string.Format("Binding to existing MonoBehaviour for type {0}: cannot locate the game object", 
					typeof(T)));
			}

			Component[] components = obj.GetComponents<Component>();
			T instance = null;
			foreach (Component comp in components) {
				instance = comp as T;
				if (instance != null) {
					break;
				}
			}

			if (instance == null) {
				throw new MindiException(string.Format("Binding to existing MonoBehaviour for type {0}: cannot locate a component of this type on the object {1}", 
					typeof(T), obj.name));
			}
				
			return instance;
		}

		private T ResolveResource<T, TInstance>(string path) 
				where T:class where TInstance:MonoBehaviour, T
		{
			GameObject prefab = Resources.Load<GameObject>(path);

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
