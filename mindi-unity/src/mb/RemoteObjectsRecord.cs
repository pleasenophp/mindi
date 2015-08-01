using System;
using System.Collections.Generic;
using UnityEngine;
using MinDI.StateObjects;
using MinDI.Introspection;

namespace MinDI.StateObjects {

	public class RemoteObjectsRecord : RemoteObjectsRecordRoot {
		private IList<object> objects;
		private IDictionary<Type, IList<object>> typedObjects;

		public RemoteObjectsRecord() {
			objects = new List<object>();
			typedObjects = new Dictionary<Type, IList<object>>();
		}

		public override void Register(object obj) {
			if (obj == null) {
				return;
			}
				
			base.Register(obj);
			objects.Add(obj);

			RegisterTypedObject(obj);
		}
			
		public override void DestroyAll() {
			IRemoteObjectsHash objectsHash = context.Resolve<IRemoteObjectsHash>();

			foreach (object o in objects) {
				DestroyObject(o, objectsHash);
			}

			objects.Clear();
			typedObjects.Clear();
		}

		public override void DestroyByType<T>(Func<T, bool> condition) {
			IRemoteObjectsHash objectsHash = null;

			IList<object> typedList;
			typedObjects.TryGetValue(typeof(T), out typedList);
			if (typedList == null) {
				return;
			}

			foreach (T obj in new List<object>(typedList)) {
				if (!condition(obj)) {
					continue;
				}

				if (objectsHash == null) {
					objectsHash = context.Resolve<IRemoteObjectsHash>();
				}
				typedList.Remove(obj);
				DestroyObject(obj, objectsHash);
			}
		}

		private void RegisterTypedObject(object obj) {
			IList<object> typedList;
			Type type = obj.GetType();
			if (!typedObjects.TryGetValue(type, out typedList)) {
				typedList = new List<object>();
			}
			typedList.Add(obj);
			typedObjects[type] = typedList;
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

