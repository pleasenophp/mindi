using System;
using MinDI.StateObjects;
using MinDI.Introspection;

namespace MinDI {
	[Serializable]
	public sealed class ContextDescriptor {
		private DIState _state = DIState.NotResolved;

		[NonSerialized] private IDestroyingFactory _factory;

		[NonSerialized] private IDIContext _context;

		[NonSerialized] private IBinding _descriptor = new Binding();

		public IDestroyingFactory factory {
			get { return _factory; }
			set { _factory = value; }
		}

		public IDIContext context {
			get { return _context; }
			set {
				if (_context != null) {
					throw new MindiException("Attempt to alter context on ContextObject");
				}
				_context = value;
			}
		}

		public DIState diState {
			get { return _state; }
			set { _state = value; }
		}

		public IBinding bindingDescriptor {
			get { return _descriptor; }
			set { _descriptor = value; }
		}
	}
}