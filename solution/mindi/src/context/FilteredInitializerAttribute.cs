using System;

namespace MinDI.Context {

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class FilteredInitializerAttribute : Attribute {
		public string name { get; private set; }

		public FilteredInitializerAttribute(string name) {
			this.name = name;
		}
	}
}

