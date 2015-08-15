using minioc.resolution.dependencies;

namespace minioc {

	// TODO - remove as it seems to be not used
	public static class Dependencies {
		/// <summary>
		/// Creates a Dependency for a parameter of type T
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static DependencyStub ForType<T>() {
			ExplicitDependency dependency = new ExplicitDependency();
			dependency.setTarget(new TypeDependencyTarget(typeof(T)));
			return dependency;
		}

		/// <summary>
		/// Creates a Dependency for a parameter of the given name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static DependencyStub ForName(string name) {
			ExplicitDependency dependency = new ExplicitDependency();
			dependency.setTarget(new NameDependencyTarget(name));
			return dependency;
		}
	}
}