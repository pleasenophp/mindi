using MinDI.Binders;

namespace MinDI {
	public static class IDIContextUnityExtensions {
		public static MonoBehaviourBinder mbSingleton(this IDIContext context) {
			return new MonoBehaviourBinder(context);
		}

		public static MonoBehaviourBinder mbMultiple(this IDIContext context) {
			return new MonoBehaviourMultipleBinder(context);
		}

		public static MonoBehaviourBinder mbs(this IDIContext context) {
			return new MonoBehaviourBinder(context);
		}

		public static MonoBehaviourBinder mbm(this IDIContext context) {
			return new MonoBehaviourMultipleBinder(context);
		}

	}
}

