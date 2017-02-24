using System;
using System.Collections.Generic;
using UnityEngine;
using MinDI.StateObjects;
using MinDI.Introspection;

namespace MinDI.StateObjects {

	public class RemoteObjectsDestroyer : IRemoteObjectsDestroyer {
		public void Destroy(object o, IRemoteObjectsHash objectsHash) {
			var mb = o as MonoBehaviour;
			if (mb != null) {
				DestroyMB(mb, objectsHash);
				return;
			}

			var fobj = o as FactoryObjectRecord;
			if (fobj != null) {
				DestroyFactoryObject(fobj);
			}

			var obj = o as UnityEngine.Object;
			if (obj != null) {
				DestroyDefault(obj, objectsHash);
			}
		}

		private void DestroyMB(MonoBehaviour mb, IRemoteObjectsHash objectsHash) {
			objectsHash.Remove(mb.gameObject);
		    var ct = mb as IDIClosedContext;
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
			objectsHash.Remove(obj);
		    UnityEngine.Object.Destroy(obj);
		}
	}
}

