using MinDI.Binders;

namespace MinDI {
	public static class IDIContextExtensions {
		public static MultipleBinder multiple(this IDIContext context) {
			return new MultipleBinder(context);
		}

		public static SingletonBinder singletone(this IDIContext context) {
			return new SingletonBinder(context);
		}

		public static MultipleBinder m(this IDIContext context) {
			return new MultipleBinder(context);
		}

		public static SingletonBinder s(this IDIContext context) {
			return new SingletonBinder(context);
		}
	}
}

