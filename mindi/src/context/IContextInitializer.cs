using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using minioc;
using minioc.attributes;
using minioc.context.bindings;
using minioc.resolution.instantiator;
using System.Reflection;

namespace MinDI.Context {
	public interface IContextInitializer {
		void Initialize(MiniocContext context);
	}
}

