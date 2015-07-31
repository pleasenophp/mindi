using System;
using System.Collections;
using minioc;

using minioc.context.bindings;
using minioc.resolution.instantiator;
using minioc.resolution.dependencies;

namespace MinDI.Binders {

	public static class BindHelper {

		public static string GetDefaultBindingName<T>(IDIContext context) {
			string contextName = (string.IsNullOrEmpty(context.name))?"context":context.name;
			string name = string.Format("{0}_{1}_{2}", "#binding", contextName, typeof(T).FullName);
			return name;
		}
	}
}