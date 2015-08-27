using System;
using System.Collections;
using minioc;

using minioc.context.bindings;
using minioc.resolution.instantiator;
using MinDI.StateObjects;
using MinDI.Introspection;

namespace MinDI.Binders {

	// That's usefull for the singleton classes 
	public partial class SingletonBinder : BaseDIBinder
	{
		private object instance = null;

		public SingletonBinder(IDIContext context) : base (context) {
			this.instantiationType = InstantiationType.Concrete;
		}
			
		protected override T Resolve<T> (Func<T> create)
		{
			if (instance == null) {
				instance = create ();
			}
			return instance as T;
		}

		private IBinding InternalBindInstance<T> (T instance, string name=null)
		{
			if (string.IsNullOrEmpty(name)) {
				name = BindHelper.GetDefaultBindingName<T>(context);
			}

			return Bindings.ForType<T> (name).ImplementedByInstance(instance)
				.SetDescriptor(this.context, InstantiationType.Instance, null);
		}
	}
}
