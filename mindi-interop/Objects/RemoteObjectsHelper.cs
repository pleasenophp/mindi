﻿using System;
using System.Collections.Generic;

namespace MinDI.StateObjects {
	public static class RemoteObjectsHelper {
		private static object locker = new object();

		private static IList<IRemoteObjectsValidator> validators = new List<IRemoteObjectsValidator>();

		public static void AddValidator(IRemoteObjectsValidator validator) {
			validators.Add(validator);
		}

		public static bool IsRemoteObject(object obj) {
			lock (locker) {
				foreach (IRemoteObjectsValidator validator in validators) {
					if (validator.IsRemoteObject(obj)) {
						return true;
					}
				}
				return false;
			}
		}


	}
}

