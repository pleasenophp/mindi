using UnityEngine;
using System.Collections;
using MinDI;

namespace MinDI {

	public class SceneObject : ContextObject, ISceneObject {
		public string name {get; set;}
	}

}
