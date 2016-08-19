using System;
using MinDI.Introspection;
using System.Collections.Generic;

namespace MinDI {
	public interface IBinding {
		string name { get; }
		IList<Type> types { get; }
		bool makeDefault { get; }

		InstantiationType instantiationType { get; }
		InstantiationType genericInstantiation { get; }

		object instance { get; }
		IDIContext context { get; }
		Func<object> factory { get; }

		object Resolve();
	}
}

