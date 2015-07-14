using System;
using System.Collections.Generic;


namespace MinDI.StateObjects {
	
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
	}
}
