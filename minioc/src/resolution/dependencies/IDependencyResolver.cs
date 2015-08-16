using System;

namespace minioc.resolution.dependencies {
	public interface IDependencyResolver {
		T Resolve<T>(bool omitInjectDependencies = false);

		T Resolve<T>(string name, bool omitInjectDependencies = false);

		object Resolve(Type type, string name =null, bool omitInjectDependencies = false);

		object TryResolve(Type type, string name = null, bool omitInjectDependencies = false);
	}
}