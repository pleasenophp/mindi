using System;
using MinDI.StateObjects;
using UnityEngine;

namespace MinDI {
	/// <summary>
	/// Derive your MonoBehaviours that work with MinDI from this class
	/// </summary>
	public abstract class ContextMonoBehaviour : MonoBehaviour, IDIClosedContext {
		[NonSerialized] private ContextDescriptor _descriptor = new ContextDescriptor();

		protected bool isInjected = false;

		[Injection] protected IDIContext contextInjection {
			set { _descriptor.context = value; }
		}


		void IDIClosedContext.AfterInjection() {
			if (isInjected) return;
			OnInjected();
			isInjected = true;
		}

		void IDIClosedContext.BeforeFactoryDestruction() {
			if (!isInjected) return;
			OnDestruction();
			isInjected = false;
		}

		bool IDIClosedContext.IsValid() {
			return _descriptor != null;
		}

		void IDIClosedContext.Invalidate() {
			this._descriptor = null;
		}

		ContextDescriptor IDIClosedContext.descriptor {
			get { return _descriptor; }
		}

		protected virtual void OnInjected() {
		}

		protected virtual void OnDestruction() {
		}
	}
}