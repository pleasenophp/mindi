using System;
using System.Collections;
using MinDI.StateObjects;
using UnityEngine;
using MinDI.Introspection;

namespace MinDI {

	/// <summary>
	/// Derive your MonoBehaviours that work with MinDI from this class
	/// </summary>
	public abstract class ContextMonoBehaviour : MonoBehaviour, IDIClosedContext {
		private DIState _state = DIState.NotResolved;

		[NonSerialized]
		private IDIContext _context;

		[NonSerialized]
		private BindingDescriptor _descriptor = new BindingDescriptor();


		#region IDIClosedContext implementation


		[Injection]
		public IDIContext contextInjection {
			set {
				if (_context != null) {
					throw new MindiException("Attempt to alter context on ContextObject");
				}
				_context = value;
			}
		}

		DIState IDIClosedContext.diState {
			get {
				return _state;
			}
			set {
				_state = value;
			}
		}

		void IDIClosedContext.AfterInjection() {
			IRemoteObjectsRecord remoteRecord = _context.Resolve<IRemoteObjectsRecord>();
			remoteRecord.Register(this);
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
	}
}
