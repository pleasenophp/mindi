using System.Collections.Generic;
using minioc.resolution.dependencies;
using MinDI;

namespace minioc.resolution.injection {
	internal interface IInjectionStrategy {
		bool IsVoid();
		void inject(object instance, IDependencyResolver dependencyResolver, IDependencyResolver explicitDependencies);
	}
}