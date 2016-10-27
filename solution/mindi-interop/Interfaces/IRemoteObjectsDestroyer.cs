using System;
using System.Collections.Generic;

namespace MinDI.StateObjects {
	public interface IRemoteObjectsDestroyer {
		void Destroy(object instance, IRemoteObjectsHash hash);
	}
		
}

