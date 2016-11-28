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
			objectsHash.hash.Remove(mb.gameObject.GetInstanceID());
		    IDIClosedContext ct = mb as IDIClosedContext;
		    if (ct != null)
		    {
		        ct.BeforeFactoryDestruction();
		    }

			GameObject.Destroy(mb.gameObject);
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

