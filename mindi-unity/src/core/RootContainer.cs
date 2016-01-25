using UnityEngine;
using System.Collections;

namespace MinDI.Unity {

	internal static class RootContainer {
		public static IDIContext context {get; set;}
		public static string overrideAutoStartScene {get; set;}
	}

}
