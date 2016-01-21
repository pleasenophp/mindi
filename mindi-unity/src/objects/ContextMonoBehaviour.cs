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
		[NonSerialized]
		private ContextDescriptor _descriptor = new ContextDescriptor();

		[Injection]
		protected IDIContext contextInjection {
			set {
				_descriptor.context = value;
			}
		}

		void IDIClosedContext.AfterInjection() {
			OnInjected();
		}

		void IDIClosedContext.BeforeFactoryDestruction() {
			OnDestruction();
		}

		bool IDIClosedContext.IsValid() {
			return _descriptor != null;
		}

		void IDIClosedContext.Invalidate() {
			this._descriptor = null;
		}

		ContextDescriptor IDIClosedContext.descriptor {
			get {
				return _descriptor;
			}
		}

		protected virtual void OnInjected() {
		}

		protected virtual void OnDestruction() {
		}
	}
}
