using System;
using System.Collections;
using minioc;
using minioc.context.bindings;
using minioc.resolution.instantiator;
using MinDI.StateObjects;
using MinDI.Factories;


namespace MinDI {

	// TODO - sort DRY with PublicContextObject
	[Serializable]
	public class ContextObject : DIStateObject, IDIClosedContext, IAutoDestructable {
		#region IAutoDestructable implementation

		[NonSerialized]
		private IDestroyingFactory _factory;
		public IDestroyingFactory factory {
			get {
				return _factory;
			}
			set {
				_factory = value;
			}
		}

		~ContextObject() {
			if (_factory != null) {
				IActionQueue queue = _context.Resolve<IActionQueue>();
				queue.Enqueue(() => _factory.DestroyInstance(this));
			}
		}

		#endregion


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
