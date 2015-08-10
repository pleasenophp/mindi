using System;

namespace MinDI {
	public static class BindingName {
		public static string ForType<T>() {
			return typeof(T).FullName;
		}

		public static string ForType(object instance) {
			return instance.GetType().FullName;
		}
	}
}

