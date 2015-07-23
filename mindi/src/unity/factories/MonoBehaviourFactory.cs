using System;
using System.Collections;
using minioc;

using minioc.context.bindings;
using minioc.resolution.instantiator;
using UnityEngine;
using System.Collections.Generic;

namespace MinDI.Factories {

	// TODO - might need to remove it after building it in context
	/// <summary>
	/// Standard factory to create mono behaviours
	/// </summary>
	public class MonoBehaviourFactory<T, TInstance> : 
		OpenContextObject, 
		IDIFactory<T> where T:class where TInstance:MonoBehaviour, T 
	{

		protected InstantiationMode instantiationMode;
		protected GameObject specifiedObject = null;
		protected string gameObjectName = null;


		public MonoBehaviourFactory(InstantiationMode instantiationMode, string gameObjectName = null) {
			this.instantiationMode = instantiationMode;
			this.gameObjectName = gameObjectName;
		}

		public MonoBehaviourFactory(InstantiationMode instantiationMode, GameObject specifiedObject) {
			if (specifiedObject == null) {
				throw new ArgumentException("Specified game object cannot be null !");
			}

			this.instantiationMode = instantiationMode;
			this.specifiedObject = specifiedObject;
		}

		public T Resolve (string name = null) {
			Reconfigure();

			GameObject obj = CreateOrFindGameObject();
			if (obj == null) {
				throw new MindiException("Failed to instantiate the game object with name: "+gameObjectName);
			}

			if (instantiationMode == InstantiationMode.SINGLETON) {
				TInstance existingComponent = obj.GetComponent<TInstance>();
				if (existingComponent != null && !IsDestroyed(existingComponent)) {
					return existingComponent;
				}
			}

			T result = obj.AddComponent<TInstance>();
			context.InjectDependencies(result);
			return result;
		}

		public T Destroy(T instance) {
			TInstance component = instance as TInstance;
			if (component == null) {
				return null;
			}

			GameObject obj = component.gameObject;
			if (obj == null) {
				return null;
			}

			// If it's specified object mode, only destroying the component, not touching the object
			if (specifiedObject != null) {
				DestroyComponent(component);
				return null;
			}

			// Else if it's binding by object name mode - destroying the game object when there is no more components on it
			List<MonoBehaviour> components = GetNotDestroyedComponents(obj);
			if (components.Count == 1) {
				DestroyComponents(components);
				GameObject.Destroy(obj);
			}
			else {
				DestroyComponent(component);
			}

			return null;
		}

		protected virtual void Reconfigure() {
		}

		private GameObject CreateOrFindGameObject() {
			if (specifiedObject != null) {
				return specifiedObject;
			}

			if (string.IsNullOrEmpty(gameObjectName)) {
				return new GameObject("_mindi_"+typeof(T).Name);
			}

			GameObject obj = GameObject.Find(gameObjectName);
			if (obj == null) {
				return new GameObject(gameObjectName);
			}
			else {
				return obj;
			}
		}

		private bool IsDestroyed(TInstance component) {
			if (component == null) {
				return true;
			}

			DIStateMonoBehaviour dismb = component as DIStateMonoBehaviour;
			if (dismb != null) {
				return dismb.__destroyed;
			}
			return false;
		}

		private void DestroyComponent(MonoBehaviour component) {
			DIStateMonoBehaviour dismb = component as DIStateMonoBehaviour;
			if (dismb != null) {
				dismb.__destroyed = true;
				Component.Destroy(component);
			}
			else {
				Debug.LogWarning(string.Format("Your component {0} doesn't implement {1} or underlaying classes. The unsafe DestroyImmediate is being used !", 
				                               component, typeof(ContextMonoBehaviour).FullName));
				Component.DestroyImmediate(component);
			}
		}

		private List<MonoBehaviour> GetNotDestroyedComponents(GameObject obj) {
			MonoBehaviour[] components = obj.GetComponents<MonoBehaviour>();
			List<MonoBehaviour> result = new List<MonoBehaviour>(components.Length);

			foreach (MonoBehaviour component in components) {
				DIStateMonoBehaviour dismb = component as DIStateMonoBehaviour;
				if (dismb == null || !dismb.__destroyed) {
					result.Add(component);
				}
			}

			return result;
		}

		private void DestroyComponents(List<MonoBehaviour> components) {
			foreach (MonoBehaviour component in components) {
				DestroyComponent(component);
			}
		}
	}
}
