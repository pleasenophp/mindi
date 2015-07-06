using System;
using System.Collections;
using minioc;
using minioc.attributes;
using minioc.context.bindings;
using minioc.resolution.instantiator;
using minioc.MinDI;

namespace MinDI.Objects {

	// This is usefull to have public context on object - e.g. for factories
	[Serializable]
	public class PublicContextObject : DIStateObject {
		
		[NonSerialized]
		private MiniocContext _context;
		
		[InjectionProperty]
		public MiniocContext context {
			get {
				return _context;
			}
			set {
				if (_context != null) {
					throw new MindiException("Attempt to alter context on ContextObject");
				}
				_context = value;
			}
		}
	}

}
