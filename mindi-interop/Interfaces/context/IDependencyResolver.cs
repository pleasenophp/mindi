using System;

namespace minioc.resolution.dependencies {
	public interface IDependencyResolver {
		object Resolve(Type type, string name, bool omitInjectDependencies);
		object TryResolve(Type type, string name, bool omitInjectDependencies);
	}
}