using System;

namespace minioc.resolution.dependencies {
	public interface IDependencyResolver {
		object TryResolve(Type type, string name, bool omitInjectDependencies);
	}
}