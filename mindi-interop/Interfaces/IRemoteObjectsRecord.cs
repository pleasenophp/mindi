using System;
using System.Collections.Generic;

namespace MinDI.StateObjects {
	public interface IRemoteObjectsRecord {
		void Register(object obj);
		void DestroyAll();
	}
		
}

