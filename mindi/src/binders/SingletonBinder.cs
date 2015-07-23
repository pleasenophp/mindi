using System;
using System.Collections;
using minioc;

using minioc.context.bindings;
using minioc.resolution.instantiator;
using MinDI.StateObjects;

namespace MinDI.Binders {

	// TODO - we should not be able to rebind the singletone object

	// That's usefull for the singleton classes 
	public class SingletonBinder : BaseDIBinder
	{
		public SingletonBinder(IDIContext context) : base (context) {}

		private object instance = null;

		/// <summary>
		/// Rebind the binding from parent context to a new binder.
		/// This can be usefull to e.g. rebind the library binding to a singletone
		/// </summary>
		/// <param name="name">Name for new binding.</param>
		/// <param name="resolutionName">Resolution name for parent binding.</param>
		/// <param name="configure">Configuration of binding.</param>
		/// <typeparam name="T">The interface type.</typeparam>
		public IBinding Rebind<T>(string name = null, string resolutionName = null, Action<IBinding> configure = null) 
			where T:class 
		{
			if (context.parent == null) {
				throw new MindiException("Called Rebind, but the parent context is null");
			}

			return this.Bind<T> (()=>context.parent.Resolve<T>(resolutionName, true), 
				name, configure);
		}

		protected override T Resolve<T> (Func<T> create)
		{
			if (instance == null) {
				instance = create ();
				InjectCreatorContext();

			}
			return instance as T;
		}

		private void InjectCreatorContext() {
			IDIClosedContext contextObject = instance as IDIClosedContext;
			if (contextObject != null) {
				contextObject.stCreatorContext = this.context;
			}
		}
	}
}
