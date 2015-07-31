using UnityEngine;
using System.Collections;
using MinDI;
using MinDI.Context;

namespace MinDI {

	public class UnityContextStart : MonoBehaviour {

		public virtual IDIContext CreateContext() {
			IDIContext context = ContextHelper.CreateContext ();
			ContextBuilder.Initialize<IGlobalContextInitializer> (context);
			return context;
		}

		// All the global initialization happens here
		public virtual void GlobalStart(IDIContext context) {
		}

	}

}
