using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

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

		public void DestroyAll() {
			foreach (Object obj in objects) {
				if (obj == null) {
					continue;
				}
					
				ContextMonoBehaviour contextMb = obj as ContextMonoBehaviour;
				if (contextMb != null) {
					contextMb.__destroyed = true;
				}
				GameObject.Destroy(obj);
			}

			objects.Clear();
		}

	}
}

