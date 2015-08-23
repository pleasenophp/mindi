using System.Collections.Generic;
using System.Reflection;
using minioc.misc;
using MinDI;
using minioc.resolution.dependencies;

namespace minioc.resolution.injection {
	internal class PrimitiveInjectionStrategy : IInjectionStrategy {
		public void inject(object instance, IDependencyResolver dependencyResolver, IDependencyResolver explicitDependencies) {
	    }

		public bool IsVoid() {
			return false;
		}
	}
}