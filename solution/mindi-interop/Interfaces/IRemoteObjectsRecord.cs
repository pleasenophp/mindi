using System;

namespace MinDI.StateObjects {
	public interface IRemoteObjectsRecord {
		void Register(object obj);
		void DestroyByType<T>(Func<T, bool> condition) where T:class;
		void DestroyAll();
	}
		
}

