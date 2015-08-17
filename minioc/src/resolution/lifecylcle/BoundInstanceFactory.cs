using System.Collections.Generic;
using minioc.resolution.dependencies;
using minioc.resolution.core;
using MinDI;

namespace minioc.resolution.lifecycle {
	internal interface BoundInstanceFactory {
		object getInstance(InjectionContext injectionContext);
	}
}