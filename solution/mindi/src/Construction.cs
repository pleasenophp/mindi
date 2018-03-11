using MinDI.Resolution;
using minioc.resolution.dependencies;
using minioc;

namespace MinDI {
	public class Construction : IConstruction {
		private readonly IDIContext explicitContext;

		private Construction() {
			explicitContext = new MiniocContext("__construction");
		}

		public static Construction Empty() {
			return new Construction();
		}

		public static Construction ForType<T>(T instance) {
			return ForType<T>(null, instance);
		}

		public static Construction ForType<T>(string name, T instance) {
			Construction construction = new Construction();
			construction.explicitContext.s().BindInstance<T>(instance, name);
			return construction;
		}

		public static Construction For(string name, object instance) {
			Construction construction = new Construction();
			construction.explicitContext.s().BindInstance<object>(instance, name);
			return construction;
		}

		public Construction AndType<T>(T instance) {
			return AndType<T>(null, instance);
		}

		public Construction And(string name, object instance) {
			this.explicitContext.s().BindInstance<object>(instance, name);
			return this;
		}

		public Construction AndType<T>(string name, T instance) {
			this.explicitContext.s().BindInstance<T>(instance, name);
			return this;
		}

		IDependencyResolver IConstruction.GetExplicitContext() {
			return explicitContext as IDependencyResolver;
		}
	}
}

