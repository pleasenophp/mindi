using System;
using System.Collections.Generic;
using minioc.context;
using minioc.context.bindings;
using minioc.misc;
using minioc.resolution.core;
using minioc.resolution.dependencies;

namespace minioc.MinDI {
	public enum DIState {
		NotResolved,
		Resolving,
		Resolved
	}
}
