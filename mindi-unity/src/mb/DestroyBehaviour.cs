using System;
using UnityEngine;

namespace MinDI.StateObjects {
	public class DestroyBehaviour : MonoBehaviour {
		private MBInstantiationType _instantiationType = MBInstantiationType.None;
		public MBInstantiationType instantiationType {
			get {
				return _instantiationType;
			}
			set {
				_instantiationType = value;
			}
		}
	}
}

