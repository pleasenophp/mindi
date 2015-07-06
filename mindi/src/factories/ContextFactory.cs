using System;
using System.Collections;
using minioc;
using minioc.attributes;
using minioc.context.bindings;
using minioc.resolution.instantiator;
using MinDI.Objects;


namespace MinDI.Factories {
	/// <summary>
	/// Standard factory to resolve an object from context
	/// </summary>
	public class ContextFactory<T> : PublicContextObject, IDIFactory<T> where T:class
	{
		public T Resolve (string name = null) {
			if (string.IsNullOrEmpty(name)) {
				return context.Resolve<T>();
			}
			else {
				return context.Resolve<T>(name);
			}
		}
	}
}
