using System;
using MinDI.StateObjects;

namespace MinDI {
	[Serializable]
	public abstract class RemotingContextObject : MarshalByRefObject, IDIClosedContext {
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
			return this._descriptor != null;
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