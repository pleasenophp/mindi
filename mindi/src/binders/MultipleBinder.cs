using System;
using System.Collections;
using minioc;
using minioc.context.bindings;
using MinDI.Introspection;


namespace MinDI.Binders {

	public class MultipleBinder : BaseDIBinder
	{

		public MultipleBinder(IDIContext context) : base (context) {
			this.instantiationType = InstantiationType.Abstract;
		}

	}

}
