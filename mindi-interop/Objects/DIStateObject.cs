using System;
using System.Collections.Generic;


namespace MinDI.StateObjects {


	// TODO - sort out the objects hierarchy.
	// Leave: ContextObject, ContextMonoBehaviour, OpenContextObject
	[Serializable]
	public abstract class DIStateObject : IDIStateObject {
		private DIState _state = DIState.NotResolved;
		DIState IDIStateObject.diState {
			get {
				return _state;
			}
			set {
				_state = value;
			}
		}

		public virtual void AfterInjection() {
		}
	}
}
