using System;
using System.Collections.Generic;
using MinDI.StateObjects;
using MinDI.Introspection;

namespace MinDI {

	[Serializable]
	public abstract class ContextObject : IDIClosedContext {

		[NonSerialized]
		private ContextDescriptor _descriptor = new ContextDescriptor();

		[Injection]
		public IDIContext contextInjection {
			set {
				_descriptor.context = value;
			}
		}


		ContextDescriptor IDIClosedContext.descriptor {
			get {
				return _descriptor;
			}
		}

		void IDIClosedContext.AfterInjection() {
			OnInjected();
		}

		void IDIClosedContext.BeforeFactoryDestruction() {
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
