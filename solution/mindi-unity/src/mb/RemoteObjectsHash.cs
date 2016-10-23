using System;
using System.Collections.Generic;

namespace MinDI.StateObjects {
	public class RemoteObjectsHash : IRemoteObjectsHash {
		public HashSet<int> hash {get; set;}

		public RemoteObjectsHash() {
			hash = new HashSet<int>();
		}

		public void Register(object instance) {
			UnityEngine.Object obj = instance as UnityEngine.Object;
			if (obj != null) {
				hash.Add(obj.GetInstanceID());
			}
		}
	}
}

