using System;
using System.Collections.Generic;
using MinDI.Resolution;
using minioc.resolution.dependencies;

namespace MinDI {
	public class Construction : IConstruction {
		private IDIContext explicitContext;

		private Construction() {
			explicitContext = ContextHelper.CreateContext();
		}

		public static Construction For<T>(T instance, string name = null) {
			Construction construction = new Construction();
			construction.explicitContext.s().BindInstance<T>(instance, name);
			return construction;
		}

		public Construction And<T>(T instance, string name = null) {
			this.explicitContext.s().BindInstance<T>(instance, name);
			return this;
		}

		IDependencyResolver IConstruction.GetExplicitContext() {
			return explicitContext as IDependencyResolver;
		}
	}
}

