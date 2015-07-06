using System;
using System.Collections;
using minioc;
using minioc.attributes;
using minioc.context.bindings;
using minioc.resolution.instantiator;

namespace MinDI.Binders {

	public class SingletonBinder : MultipleBinder
	{
		protected override void ConfigureBinding (Binding binding)
		{
			binding.SetInstantiationMode (InstantiationMode.SINGLETON);
		}
	}
}