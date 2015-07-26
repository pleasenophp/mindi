using System;
using System.Collections.Generic;
using UnityEngine;
using MinDI.StateObjects;
using MinDI.Introspection;

namespace MinDI.StateObjects {

	public class RemoteObjectsRecord : OpenContextObject, IRemoteObjectsRecord {
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
			IRemoteObjectsHash objectsHash = context.Resolve<IRemoteObjectsHash>();

			foreach (object o in objects) {
				UnityEngine.Object obj = o as UnityEngine.Object;
				if (obj == null) {
					DestroyFactoryObject(o);
					continue;
				}
					
				MonoBehaviour mb = obj as MonoBehaviour;
				if (mb != null) {
					DestroyBehaviour destroyBehaviour = mb.GetComponent<DestroyBehaviour>();
					if (destroyBehaviour == null || destroyBehaviour.instantiationType == MBInstantiationType.ExistingObject) {
						GameObject.Destroy(mb);
					}
					else if (destroyBehaviour.instantiationType == MBInstantiationType.NewObject) {
						objectsHash.hash.Remove(mb.gameObject.GetInstanceID());
						GameObject.Destroy(mb.gameObject);
					}
				}
				else {
					objectsHash.hash.Remove(obj.GetInstanceID());
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

