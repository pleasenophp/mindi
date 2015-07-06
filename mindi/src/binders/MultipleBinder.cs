using System;
using System.Collections;
using minioc;
using minioc.attributes;
using minioc.context.bindings;
using minioc.resolution.instantiator;


namespace MinDI.Binders {

	public class MultipleBinder : BaseDIBinder
	{
		#region IDIFactory implementation
		
		public override T Resolve<T> (Func<T> create)
		{
			return create ();
		}
		
		#endregion
	}

}
