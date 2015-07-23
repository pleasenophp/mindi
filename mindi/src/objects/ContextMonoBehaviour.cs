using System;
using System.Collections;
using MinDI.StateObjects;


namespace MinDI {

	/// <summary>
	/// Derive your MonoBehaviours that work with MinDI from this class
	/// </summary>
	public class ContextMonoBehaviour : DIStateMonoBehaviour, IDIClosedContext {
		[NonSerialized]
		private IDIContext _context;

		[Injection]
		private IDIContext contextInjection {
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
