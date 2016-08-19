using System;
using System.Collections;
using minioc;


using MinDI.Context;
using MinDI.StateObjects;
using MinDI.Introspection;
using MinDI.Factories;
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
