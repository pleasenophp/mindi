using System;

namespace MinDI.Introspection {
	public class BindingDescriptor {
		public BindingType bindingType { get; set; }
		public InstantiationType instantiationType { get; set; }
		public string name { get; set; }
		public bool isDefault { get; set; }
		public IDIContext context { get; set; }
		public Func<object> factory { get; set;}

		public BindingDescriptor() {
			bindingType = BindingType.None;
			instantiationType = InstantiationType.None;
			isDefault = false;
		}
	}
}

