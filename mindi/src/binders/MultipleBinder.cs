using System;
using System.Collections;
using minioc;
using minioc.context.bindings;
using minioc.resolution.instantiator;


namespace MinDI.Binders {

	public class MultipleBinder : BaseDIBinder
	{

		public MultipleBinder(IDIContext context) : base (context) {}
		
		protected override T Resolve<T> (Func<T> create)
		{
			return create ();
		}

	}

}
