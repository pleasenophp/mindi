using System;
using System.Collections.Generic;
using UnityEngine;
using MinDI.StateObjects;
using MinDI.Introspection;

namespace MinDI.StateObjects {

	public class RemoteObjectsRecord : RemoteObjectsRecordRoot {
		private IList<object> objects;

		public RemoteObjectsRecord() {
			objects = new List<object>();
		}

		public override void Register(object obj) {
			if (obj == null) {
				return;
			}
				
			base.Register(obj);
			objects.Add(obj);
		}
			
		public override void DestroyAll() {
			IRemoteObjectsHash objectsHash = context.Resolve<IRemoteObjectsHash>();

			foreach (object o in objects) {
				DestroyObject(o, objectsHash);
			}

			objects.Clear();
		}

		private void DestroyObject(object o, IRemoteObjectsHash objectsHash) {
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

