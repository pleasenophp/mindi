using System;
using System.Collections;
using minioc;
using minioc.context.bindings;
using minioc.resolution.instantiator;
using MinDI.StateObjects;


namespace MinDI {

	// TODO - sort DRY with PublicContextObject
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

		[NonSerialized]
		private IDIContext _stCreatorContext = null;
		IDIContext IDIClosedContext.stCreatorContext {
			get {
				return _stCreatorContext;
			}
			set {
				_stCreatorContext = value;
			}
		}

		#endregion
	}
}
