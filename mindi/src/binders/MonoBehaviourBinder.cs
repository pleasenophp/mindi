using System;
using System.Collections;
using minioc;
using minioc.context.bindings;
using minioc.resolution.instantiator;
using UnityEngine;

namespace MinDI.Binders {

	public class MonoBehaviourBinder : OpenContextObject {
		private IDIBinder baseBinder;

		public MonoBehaviourBinder(IDIContext context) : this (context, InstantiationMode.SINGLETON) {
		}

		protected MonoBehaviourBinder(IDIContext context, InstantiationMode mode) {
			this.context = context;

			var b = context.CreateBindHelper();

			if (mode == InstantiationMode.SINGLETON) {
				baseBinder = b.singleton;
			}
			else if (mode == InstantiationMode.MULTIPLE) {
				baseBinder = b.multiple;
			}
		}



		// TODO - add different binding types - for prefab, etc
		// TODO - add resolution of the ContextType, and bind depending on it as DontDestroyOnLoad
		// ContextType: Normal (not allowed to bind MB at all), UnityRoot (always create MB as DontDestroyOnLoad, 
		// Unity - Create MB normally
		// TODO - add reporting of every new mono behaviour creation to the special ContextDescriptor object.
		// Thus each factory for mono behaviour can have an instance of this object on it's own context, to 
		// track all the monobehaviours that are created during one instantiation procedure

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
			return obj.AddComponent<TInstance>();
		}

		private T ResolveResource<T, TInstance>(string path) 
				where T:class where TInstance:MonoBehaviour, T
		{
			GameObject prefab = Resources.Load<GameObject>(path);
			if (prefab == null) {
				throw new MindiException("Cannot load the resource by path "+path);
			}

			GameObject obj = (GameObject)GameObject.Instantiate(prefab);
			obj.name = typeof(TInstance).Name;
			return obj.GetComponent<TInstance>();
		}
	}

}
