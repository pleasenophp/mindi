namespace MinDI.Resolution {
	
	public sealed class DynamicResolver<T> : OpenContextObject, IDynamicInjection<T> {
		public T Resolve (string name) {
			return context.Resolve<T>(name);
		}
	}
}
