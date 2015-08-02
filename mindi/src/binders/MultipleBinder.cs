using System;
using System.Collections;
using minioc;
using minioc.context.bindings;
using minioc.resolution.instantiator;
using MinDI.Introspection;


namespace MinDI.Binders {

	public class MultipleBinder : BaseDIBinder
	{


		public MultipleBinder(IDIContext context) : base (context) {
			this.instantiationType = InstantiationType.Abstract;
		}
		
		protected override T Resolve<T> (Func<T> create)
		{
			return create ();
		}

	}

}
