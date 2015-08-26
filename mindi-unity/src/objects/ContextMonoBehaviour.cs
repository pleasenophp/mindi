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
		private IDestroyingFactory _factory;

		[NonSerialized]
		private BindingDescriptor _descriptor = new BindingDescriptor();


		#region IDIClosedContext implementation

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

		void IDIClosedContext.BeforeFactoryDestruction() {
			OnDestruction();
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

		protected virtual void OnDestruction() {
		}
	}
}
