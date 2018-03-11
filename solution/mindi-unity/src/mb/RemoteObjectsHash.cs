using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MinDI.StateObjects {
	public class RemoteObjectsHash : IRemoteObjectsHash {
		public HashSet<int> hash { get; set; }

		public RemoteObjectsHash() {
			hash = new HashSet<int>();
		}

		public void Register(object instance) {
			var obj = instance as Object;

			if (obj != null) {
				ProcessRecursively(obj, add: true);
			}
		}

		public bool Contains(int id) {
			return this.hash.Contains(id);
		}

		public void Remove(object instance) {
			var obj = instance as Object;
			if (obj != null) {
				ProcessRecursively(obj, add: false);
			}
		}

		private GameObject GetGameObject(Object o) {
			var gameObject = o as GameObject;
			if (gameObject != null) {
				return gameObject;
			}

			var behaviour = o as MonoBehaviour;
			if (behaviour != null) {
				return behaviour.gameObject;
			}

			return null;
		}

		private void ProcessRecursively(Object o, bool add) {
			AddOrRemove(o, add);

			GameObject go = GetGameObject(o);
			if (go == null) {
				return;
			}

			foreach (Transform child in go.transform) {
				ProcessRecursively(child.gameObject, add);
			}
		}

		private void AddOrRemove(Object o, bool add) {
			if (add) {
				hash.Add(o.GetInstanceID());
			}
			else {
				hash.Remove(o.GetInstanceID());
			}
		}
	}
}