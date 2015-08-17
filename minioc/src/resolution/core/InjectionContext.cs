using System;
using System.Collections.Generic;
using minioc.resolution.dependencies;
using minioc.resolution.instantiator;
using MinDI;
using MinDI.Resolution;

namespace minioc.resolution.core {
	internal interface InjectionContext {
		object createInstance(Type type, Instantiator instantiator, IList<IDependency> dependencies);

		void injectDependencies(object instance, IConstruction construction);
	}
}