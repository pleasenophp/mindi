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
			_multiple = context.CreateBinder<MultipleBinder> ();
		}
			
		public MultipleBinder multiple {
			get {
				return _multiple;
			}
		}

		public SingletonBinder singleton {
			get {
				return context.CreateBinder<SingletonBinder> ();
			}
		}
	}
}