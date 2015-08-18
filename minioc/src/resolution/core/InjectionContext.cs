using System;
using System.Collections.Generic;
using minioc.resolution.dependencies;
using minioc.resolution.instantiator;
using MinDI;
using MinDI.Resolution;

namespace minioc.resolution.core {
	internal interface InjectionContext {
		void injectDependencies(object instance, Func<IConstruction> construction);
	}
}