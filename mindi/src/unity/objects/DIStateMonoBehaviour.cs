using System;
using System.Collections.Generic;
using minioc.context;
using minioc.context.bindings;
using minioc.misc;
using minioc.resolution.core;
using minioc.resolution.dependencies;
using UnityEngine;
using MinDI.StateObjects;

namespace MinDI.Objects
{
	public abstract class DIStateMonoBehaviour : MonoBehaviour, IDIStateObject {
		private DIState _state = DIState.NotResolved;
		DIState IDIStateObject.diState {
			get {
				return _state;
			}
			set {
				_state = value;
			}
		}


		private bool _destroyed = false;
		public bool __destroyed {
			get {
				return _destroyed;
			}
			set {
				_destroyed = value;
			}
		}
	}
}
