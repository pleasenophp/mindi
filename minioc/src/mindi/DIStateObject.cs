using System;
using System.Collections.Generic;
using minioc.context;
using minioc.context.bindings;
using minioc.misc;
using minioc.resolution.core;
using minioc.resolution.dependencies;

namespace minioc.MinDI
{
	
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
