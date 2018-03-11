using minioc.resolution.dependencies;

namespace minioc.resolution.injection {
	internal interface IInjectionStrategy {
		bool IsVoid();
		void inject(object instance, IDependencyResolver dependencyResolver, IDependencyResolver explicitDependencies);
	}
}