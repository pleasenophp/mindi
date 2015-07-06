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
		private SingletonBinder _singleton;

		public BindHelper(MiniocContext context) {
			this.context = context;
			_multiple = context.CreateBinder<MultipleBinder> ();
			_singleton = context.CreateBinder<SingletonBinder> ();
		}
			
		public MultipleBinder multiple {
			get {
				return _multiple;
			}
		}

		public SingletonBinder singleton {
			get {
				return _singleton;
			}
		}

		public GroupSingletonBinder group {
			get {
				return context.CreateBinder<GroupSingletonBinder> ();
			}
		}
	}
}