using System;
using System.Collections;
using minioc;
using minioc.context.bindings;
using minioc.resolution.instantiator;
using MinDI.StateObjects;


namespace MinDI {

	/// <summary>
	/// Context object is an automatical memory of context on the object.
	/// Derive your MinDI objects from this class if you don't want to loose the context in some situations
	/// If you want an object with context access use this. 
	/// Usually it's a factory that should have such a privilegy.
	/// </summary>
	[Serializable]
	public class OpenContextObject : DIStateObject {
		
		[NonSerialized]
		private IDIContext _context;
		
		[Injection]
		public IDIContext context {
			get {
				return _context;
			}
			set {
				if (_context != null) {
					throw new MindiException("Attempt to alter context on ContextObject");
				}
				_context = value;
				OnContextInjected();
			}
		}

		protected virtual void OnContextInjected() {
		}
	}

}
