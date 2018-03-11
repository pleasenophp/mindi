using System;
using System.Collections.Generic;
using minioc.resolution.injection;
using MinDI.Resolution;

namespace minioc.resolution.core {
	internal interface InjectionContext {
		IList<IInjectionStrategy> getInjectionStrategies(object instance);
		void injectDependencies(object instance, IList<IInjectionStrategy> injectionStrategies, Func<IConstruction> construction);
	}
}