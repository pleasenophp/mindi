using System;
using System.Collections;
using minioc;

using minioc.context.bindings;
using minioc.resolution.instantiator;
using MinDI.StateObjects;
using MinDI.Introspection;

namespace MinDI.Binders {

	// That's usefull for the singleton classes 
	public class SingletonBinder : BaseDIBinder
	{
		public SingletonBinder(IDIContext context) : base (context) {
			this.instantiationType = InstantiationType.Concrete;
		}

		private object instance = null;

		protected override T Resolve<T> (Func<T> create)
		{
			if (instance == null) {
				instance = create ();
			}
			return instance as T;
		}
	}
}
