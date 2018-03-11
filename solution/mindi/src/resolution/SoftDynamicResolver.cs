namespace MinDI.Resolution {
	
	public sealed class SoftDynamicResolver<T> : OpenContextObject, ISoftDynamicInjection<T> {
		public T Resolve (string name) {
			return context.TryResolve<T>(name);
		}
	}
}
