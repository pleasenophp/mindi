namespace MinDI {
	/// <summary>
	/// A factory interface to produce objects
	/// </summary>
	public interface IDestroyingFactory {
		void DestroyInstance(object instance);
	}
}