using System.Collections.Generic;
using minioc.resolution.dependencies;
using MinDI;

namespace minioc.resolution.injection {
	internal interface InjectionStrategy {
		InjectorStrategyType type { get; }

		void inject(object instance, IDependencyResolver dependencyResolver, IList<IDependency> dependencies);
	}
}