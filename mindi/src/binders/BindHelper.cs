using System;
using System.Collections;
using minioc;

using minioc.context.bindings;
using minioc.resolution.dependencies;

namespace MinDI.Binders {

	public static class BindHelper {
		public static string GetDefaultBindingName(IDIContext context) {
			
			string contextName = (string.IsNullOrEmpty(context.name))?"c":context.name;
			string name = string.Format("{0}_{1}", "#", contextName);
			return name;
		}
	}
}