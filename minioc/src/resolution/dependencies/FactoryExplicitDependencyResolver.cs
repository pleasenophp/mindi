using System;

namespace minioc.resolution.dependencies {
	internal class FactoryExplicitDependencyResolver : ExplicitDependencyResolver {
		private readonly Func<object> _factory;

		public FactoryExplicitDependencyResolver(Func<object> factory) {
			_factory = factory;
		}

		public object resolve(Type type, IDependencyResolver dependencyResolver) {
			return _factory();
		}
	}
}