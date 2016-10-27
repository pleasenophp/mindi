using System;
using System.Collections.Generic;
using System.Linq;

namespace MinDI {
	public class BindingName {
		private IList<string> strings;

		private BindingName(String name) {
			strings = new List<string>{ name };
		}

		public static BindingName ForType<T>() {
			return new BindingName(typeof(T).FullName);
		}

		public BindingName AndType<T>() {
			strings.Add(typeof(T).FullName);
			return this;
		}

		public static BindingName ForType(object instance) {
			CheckNull(instance);
			return new BindingName(instance.GetType().FullName);
		}

		public BindingName AndType(object instance) {
			CheckNull(instance);
			strings.Add(instance.GetType().FullName);
			return this;
		}

		public static BindingName For(object instance) {
			CheckNull(instance);
			return new BindingName(instance.ToString());
		}

		public BindingName And(object instance) {
			CheckNull(instance);
			strings.Add(instance.ToString());
			return this;
		}

		public override string ToString() {
			string[] res = strings.ToArray();
			Array.Sort(res);
			return String.Join(":", res);
		}

		static public implicit operator string(BindingName instance) {
			return instance.ToString();
		}

		private static void CheckNull(object instance) {
			if (instance == null) {
				throw new MindiException("The object passed to BindingName was null !");
			}
		}
	}
}

