using System;
using System.Collections.Generic;
using UnityEngine;
using MinDI.StateObjects;
using MinDI.Introspection;

namespace MinDI.StateObjects {

	public class RemoteObjectsDestroyer : IRemoteObjectsDestroyer {
		public void Destroy(object o, IRemoteObjectsHash objectsHash) {
			MonoBehaviour mb = o as MonoBehaviour;
			if (mb != null) {
				DestroyMB(mb, objectsHash);
				return;
			}

			FactoryObjectRecord fobj = o as FactoryObjectRecord;
			if (fobj != null) {
				DestroyFactoryObject(fobj);
			}

			UnityEngine.Object obj = o as UnityEngine.Object;
			if (obj != null) {
				DestroyDefault(obj, objectsHash);
			}
		}

		private void DestroyMB(MonoBehaviour mb, IRemoteObjectsHash objectsHash) {
			DestroyBehaviour destroyBehaviour = mb.GetComponent<DestroyBehaviour>();
			if (destroyBehaviour == null || destroyBehaviour.instantiationType == MBInstantiationType.ExistingObject) {
				GameObject.Destroy(mb);
			}
			else if (destroyBehaviour.instantiationType == MBInstantiationType.NewObject) {
				objectsHash.hash.Remove(mb.gameObject.GetInstanceID());
				GameObject.Destroy(mb.gameObject);
			}
		}
			
		private void DestroyFactoryObject(FactoryObjectRecord obj) {
			obj.factory.DestroyInstance(obj.instance);
		}

		private void DestroyDefault(UnityEngine.Object obj, IRemoteObjectsHash objectsHash) {
			objectsHash.hash.Remove(obj.GetInstanceID());
			UnityEngine.Object.Destroy(obj);
		}


	}
}

