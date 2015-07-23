using System;
using System.Collections;
using minioc;
using minioc.context.bindings;
using minioc.resolution.instantiator;
using MinDI.StateObjects;


namespace MinDI {
	
	[Serializable]
	public class ContextObject : DIStateObject, IDIClosedContext {

		[NonSerialized]
		private IDIContext _context;

		[Injection]
		public IDIContext contextInjection {
			set {
				if (_context != null) {
					throw new MindiException("Attempt to alter context on ContextObject");
				}
				_context = value;
			}
		}

		#region IDIClosedContext implementation
		IDIContext IDIClosedContext.context {
			get {
				return _context;
			}
		}
		#endregion
	}
}
