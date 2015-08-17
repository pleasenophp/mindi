using System.Collections.Generic;
using minioc.resolution.dependencies;
using minioc.resolution.instantiator;
using minioc.resolution.core;
using MinDI;

namespace minioc.resolution.lifecycle.providers {

	// TODO - remove if not used
	internal interface BoundValueProvider {
		bool isSet { get; }

		void setInstantiator(Instantiator instantiator);

		object createInstance(InjectionContext injectionContext);
	}
}