using System;
using System.Collections;
using minioc;
using minioc.attributes;
using minioc.context.bindings;
using minioc.resolution.instantiator;

namespace MinDI.Binders {

	public class BindHelper {

		private MiniocContext context;

		private MultipleBinder _multiple;

		public BindHelper(MiniocContext context) {
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