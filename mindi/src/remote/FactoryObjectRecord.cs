using System;

namespace MinDI.StateObjects {
	public class FactoryObjectRecord {
		public object instance { get; private set;}
		public IDestroyingFactory factory { get; private set;}

		public FactoryObjectRecord(IDestroyingFactory factory, object instance) {
			this.factory = factory;
			this.instance = instance;
				
		}
	}
}

