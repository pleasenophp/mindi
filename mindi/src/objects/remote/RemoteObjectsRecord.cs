using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using MinDI.StateObjects;

namespace MinDI {

	public class RemoteObjectsRecord : IRemoteObjectsRecord {
		private IList<Object> objects;

		public RemoteObjectsRecord() {
			objects = new List<Object>();
		}

		public void Register(Object obj) {
			if (obj == null) {
				return;
			}

			objects.Add(obj);
		}

		// TODO - make this method cleaner
		public void DestroyAll() {
			foreach (Object obj in objects) {
				if (obj == null) {
					continue;
				}

				// TODO - add child factories registrations

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

	}
}

