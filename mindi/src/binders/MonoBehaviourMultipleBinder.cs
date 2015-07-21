using System;
using System.Collections;
using minioc;
using minioc.context.bindings;
using minioc.resolution.instantiator;
using UnityEngine;
using MinDI.Objects;

namespace MinDI.Binders {

	public class MonoBehaviourMultipleBinder : MonoBehaviourBinder {
		public MonoBehaviourMultipleBinder(IDIContext context) : base(context, InstantiationMode.MULTIPLE) {
			
		}
	}

}
