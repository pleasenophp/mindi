using System;
using System.Collections;
using minioc;

using minioc.context.bindings;
using minioc.resolution.instantiator;
using minioc.resolution.dependencies;

namespace MinDI.Binders {

	public static class BindHelper {

		public static string GetDefaultBindingName<T>(IDIContext context) {
			return GetDefaultBindingName(typeof(T), context);
		}

		public static string GetDefaultBindingName(Type type, IDIContext context) {
			
			string contextName = (string.IsNullOrEmpty(context.name))?"c":context.name;
			string name = string.Format("{0}_{1}_{2}", "#", contextName, type.FullName);
			return name;
		}
	}
}