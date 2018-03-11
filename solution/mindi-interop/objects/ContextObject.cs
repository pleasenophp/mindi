using System;
using MinDI.StateObjects;

namespace MinDI {
	[Serializable]
	public abstract class ContextObject : IDIClosedContext {
		protected bool isInjected = false;

		[NonSerialized] private ContextDescriptor _descriptor = new ContextDescriptor();

		[Injection] public IDIContext contextInjection {
			set { _descriptor.context = value; }
		}

		ContextDescriptor IDIClosedContext.descriptor {
			get { return _descriptor; }
		}

		void IDIClosedContext.AfterInjection() {
			if (isInjected) return;
			OnInjected();
			isInjected = true;
		}

		void IDIClosedContext.BeforeFactoryDestruction() {
			if (!isInjected) return;
			isInjected = false;
			OnDestruction();
		}

		bool IDIClosedContext.IsValid() {
			return this._descriptor != null;
		}

		void IDIClosedContext.Invalidate() {
			this._descriptor = null;
		}


		protected virtual void OnInjected() {
		}

		protected virtual void OnDestruction() {
		}
	}
}