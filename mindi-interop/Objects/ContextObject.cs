using System;
using System.Collections.Generic;
using MinDI.StateObjects;
using MinDI.Introspection;

namespace MinDI {


	[Serializable]
	public abstract class ContextObject : IDIClosedContext {
		private DIState _state = DIState.NotResolved;

		[NonSerialized]
		private IDestroyingFactory _factory;

		[NonSerialized]
		private IDIContext _context;

		[NonSerialized]
		private BindingDescriptor _descriptor = new BindingDescriptor();

		IDestroyingFactory IDIClosedContext.factory {
			get {
				return _factory;
			}
			set {
				_factory = value;
			}
		}
			
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

		DIState IDIClosedContext.diState {
			get {
				return _state;
			}
			set {
				_state = value;
			}
		}

		void IDIClosedContext.AfterInjection() {
			OnInjected();
		}

		IDIContext IDIClosedContext.context {
			get {
				return _context;
			}
		}

		BindingDescriptor IDIClosedContext.bindingDescriptor {
			get {
				return _descriptor;
			}
			set {
				_descriptor = value;
			}
		}
			
		#endregion

		protected virtual void OnInjected() {
		}
	}
}
