using System;

namespace minioc.resolution.dependencies {
	public interface DependencyResolver {
		T Resolve<T>(bool omitInjectDependencies = false);

		T Resolve<T>(string name, bool omitInjectDependencies = false);

		object Resolve(Type type, bool omitInjectDependencies = false);

		object Resolve(Type type, string name, bool omitInjectDependencies = false);
	}
}