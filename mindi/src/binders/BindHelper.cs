using System;
using System.Collections;
using minioc;

using minioc.context.bindings;
using minioc.resolution.instantiator;
using minioc.resolution.dependencies;

namespace MinDI.Binders {

	public class BindHelper {

		private IDIContext context;

		private MultipleBinder _multiple;

		public BindHelper(IDIContext context) {
			this.context = context;
			_multiple = new MultipleBinder(this.context);
		}
			
		public MultipleBinder multiple {
			get {
				return _multiple;
			}
		}

		public SingletonBinder singleton {
			get {
				return new SingletonBinder(this.context);
			}
		}

		public MonoBehaviourBinder mbSingleton {
			get {
				return new MonoBehaviourBinder(this.context);
			}
		}

		public MonoBehaviourMultipleBinder mbMultiple {
			get {
				return new MonoBehaviourMultipleBinder(this.context);
			}
		}

		public static string GetDefaultBindingName<T>(IDIContext context) {
			string contextName = (string.IsNullOrEmpty(context.name))?"context":context.name;
			string name = string.Format("{0}_{1}_{2}", "#binding", contextName, typeof(T).FullName);
			return name;
		}
	}
}