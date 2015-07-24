using System;
using System.Collections.Generic;
using UnityEngine;
using MinDI.StateObjects;

namespace MinDI.StateObjects {

	public class RemoteObjectsRecord : IRemoteObjectsRecord {
		private IList<object> objects;

		public RemoteObjectsRecord() {
			objects = new List<object>();
		}

		public void Register(object obj) {
			if (obj == null) {
				return;
			}

			objects.Add(obj);
		}

		// TODO - make this method cleaner
		public void DestroyAll() {
			foreach (object o in objects) {
				UnityEngine.Object obj = o as UnityEngine.Object;
				if (obj == null) {
					DestroyFactoryObject(o);
					continue;
				}
					
				IDIClosedContext contextObject = obj as IDIClosedContext;
				if (contextObject != null) {
					if (contextObject.stCreatorContext != null && contextObject.stCreatorContext != contextObject.context) {
						throw new MindiException(string.Format("The object {0} is already singletone on different context. Cannot destroy.", 
							obj));
					}
				}
					
				ContextMonoBehaviour contextMb = obj as ContextMonoBehaviour;
				if (contextMb != null) {
					contextMb.__destroyed = true;
				}

				MonoBehaviour mb = obj as MonoBehaviour;
				if (mb != null) {
					DestroyBehaviour destroyBehaviour = mb.GetComponent<DestroyBehaviour>();
					if (destroyBehaviour == null || destroyBehaviour.instantiationType == MBInstantiationType.ExistingObject) {
						GameObject.Destroy(mb);
					}
					else if (destroyBehaviour.instantiationType == MBInstantiationType.NewObject) {
						GameObject.Destroy(mb.gameObject);
					}
				}
				else {
					GameObject.Destroy(obj);
				}
			}

			objects.Clear();
		}

		private void DestroyFactoryObject(object o) {
			FactoryObjectRecord obj = o as FactoryObjectRecord;
			if (obj == null) {
				return;
			}

			obj.factory.DestroyInstance(obj.instance);
		}

	}
}

