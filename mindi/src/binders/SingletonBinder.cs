using System;
using System.Collections;
using minioc;
using minioc.context.bindings;
using MinDI.StateObjects;
using MinDI.Introspection;

namespace MinDI.Binders {

	// That's usefull for the singleton classes 
	public partial class SingletonBinder : BaseDIBinder
	{
		
		public SingletonBinder(IDIContext context) : base (context) {
			this.instantiationType = InstantiationType.Concrete;
		}


	}
}
