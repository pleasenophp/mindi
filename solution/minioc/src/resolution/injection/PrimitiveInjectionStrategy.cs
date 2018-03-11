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