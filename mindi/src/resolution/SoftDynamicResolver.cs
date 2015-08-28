using System;
using System.Collections;
using minioc;

using minioc.context.bindings;
using minioc.resolution.instantiator;
using MinDI.Context;
using MinDI.StateObjects;
using MinDI.Introspection;
using MinDI.Factories;


namespace MinDI.Resolution {
	
	public sealed class SoftDynamicResolver<T> : OpenContextObject, ISoftDynamicInjection<T> {
		public T Resolve (string name) {
			return context.TryResolve<T>(name);
		}
	}
}
