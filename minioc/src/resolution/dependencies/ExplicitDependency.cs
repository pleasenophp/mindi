using System;
using MinDI;

namespace minioc.resolution.dependencies {
	internal class ExplicitDependency : IDependency, DependencyStub {
		private DependencyTarget _target;
		private ExplicitDependencyResolver _resolver;

		public IDependency ResolveByName(string instanceName) {
			_resolver = new NamedExplicitDependencyResolver(instanceName);
			return this;
		}

		public IDependency ResolveByValue(object value) {
			_resolver = new ValueExplicitDependencyResolver(value);
			return this;
		}

		public IDependency ResolveByFactory(Func<object> factory) {
			_resolver = new FactoryExplicitDependencyResolver(factory);
			return this;
		}

		internal void setTarget(DependencyTarget dependencyTarget) {
			_target = dependencyTarget;
		}

		public bool tryResolveDependency(Type type, string name, out object value, DependencyResolver dependencyResolver) {
			if (_target.accept(type, name)) {
				value = _resolver.resolve(type, dependencyResolver);
				return true;
			}
			value = null;
			return false;
		}
	}
}