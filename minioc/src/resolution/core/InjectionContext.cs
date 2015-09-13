using System;
using System.Collections.Generic;
using minioc.resolution.dependencies;
using minioc.resolution.instantiator;
using MinDI;
using MinDI.Resolution;

namespace minioc.resolution.core {

	// TODO - remove the references from the places where it's not used
	internal interface InjectionContext {
		void injectDependencies(object instance, Func<IConstruction> construction);
	}
}