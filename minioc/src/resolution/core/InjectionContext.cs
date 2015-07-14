using System;
using System.Collections.Generic;
using minioc.resolution.dependencies;
using minioc.resolution.instantiator;
using MinDI;

namespace minioc.resolution.core {
	internal interface InjectionContext {
		object createInstance(Type type, Instantiator instantiator, IList<IDependency> dependencies);

		void injectDependencies(object instance, IList<IDependency> dependencies);
	}
}