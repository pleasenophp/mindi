using System;
using System.Collections;
using minioc;

using minioc.context.bindings;
using minioc.resolution.instantiator;

namespace MinDI.Binders {

	// That's usefull for the singleton classes 
	public class SingletonBinder : BaseDIBinder
	{
		private object instance = null;

		public override T Resolve<T> (Func<T> create)
		{
			if (instance == null) {
				instance = create ();
			}
			return instance as T;
		}
	}
}
