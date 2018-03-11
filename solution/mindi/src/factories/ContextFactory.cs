using System;
using MinDI.Resolution;

namespace MinDI {
	/// <summary>
	/// Standard factory to resolve an object from context
	/// </summary>
	public class ContextFactory<T> : BaseContextFactory<T>, IDIFactory<T>
		where T:class
	{
		public T Create (string name = null) {
			return CreateInstance(name);
		}

		public T Create (Func<IConstruction> construction, string name = null) {
			return CreateInstance(name, construction);
		}
	}
}
