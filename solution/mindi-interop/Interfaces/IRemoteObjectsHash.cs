namespace MinDI.StateObjects {
	public interface IRemoteObjectsHash {
		void Register(object instance);
		void Remove(object instance);
		bool Contains(int id);
	}
}

