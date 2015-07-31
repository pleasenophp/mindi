using System;
using System.Collections.Generic;

namespace MinDI.StateObjects {
	public class RemoteObjectsHash : IRemoteObjectsHash {
		public HashSet<int> hash {get; set;}

		public RemoteObjectsHash() {
			hash = new HashSet<int>();
		}
	}
}

