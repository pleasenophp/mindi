using System.Collections.Generic;
using System.Linq;

namespace MinDI.StateObjects {
	public static class RemoteObjectsHelper {
		private static readonly object locker = new object();

		private static readonly IList<IRemoteObjectsValidator> validators = new List<IRemoteObjectsValidator>();

		public static void AddValidator(IRemoteObjectsValidator validator) {
			lock (locker) {
				validators.Add(validator);
			}
		}

		public static bool IsRemoteObject(object obj) {
			lock (locker) {
				return validators.Any(validator => validator.IsRemoteObject(obj));
			}
		}
	}
}