using System;
using System.Collections.Generic;

namespace minioc {
	public static class MiniocExtensions {

		public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, Func<TValue> defaultValueFunc = null) {
			TValue val = default(TValue);
			if (!dic.TryGetValue(key, out val) && defaultValueFunc!=null) {
				val = defaultValueFunc();
			}
			return val;
		}
	}
}

