using UnityEngine;
using System;
using System.Collections;
using MinDI.Context;

namespace MinDI {

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class SceneContextAttribute : FilteredInitializerAttribute {
		public SceneContextAttribute(string name) : base(name) {
		}
	}
}
