using System;
using System.Collections;
using minioc;

using minioc.context.bindings;
using MinDI.Context;
using MinDI.StateObjects;
using MinDI.Introspection;
using MinDI.Factories;


namespace MinDI.Resolution {
	
	public sealed class DynamicResolver<T> : OpenContextObject, IDynamicInjection<T> {
		public T Resolve (string name) {
			return context.Resolve<T>(name);
		}
	}
}
