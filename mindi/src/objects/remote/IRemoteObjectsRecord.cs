using System;
using System.Collections.Generic;

namespace MinDI {
	public interface IRemoteObjectsRecord {
		void Register(UnityEngine.Object obj);
		void DestroyAll();
	}
		
}

