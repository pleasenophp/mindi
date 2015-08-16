using System;

namespace MinDI.Introspection {
	public class BindingDescriptor {
		public InstantiationType instantiationType { get; set; }
		public InstantiationType genericInstantiation { get; set; }
		public string name { get; set; }
		public bool isDefault { get; set; }
		public IDIContext context { get; set; }
		public Func<object> factory { get; set;}

		public BindingDescriptor() {
			instantiationType = InstantiationType.None;
			genericInstantiation = InstantiationType.None;
			isDefault = false;
		}

		public void InitFromGeneric(BindingDescriptor descriptor, Func<object> factory) {
			if (descriptor.genericInstantiation == InstantiationType.None) {
				throw new MindiException(string.Format("Descriptor {0} doesn't have any generic instantiation !", descriptor.name));
			}

			this.instantiationType = descriptor.genericInstantiation;
			this.name = descriptor.name;
			this.isDefault = descriptor.isDefault;
			this.context = descriptor.context;
			this.factory = factory;
		}
	}
}

