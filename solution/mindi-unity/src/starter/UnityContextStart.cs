using UnityEngine;
using System.Collections;
using MinDI;
using MinDI.Context;
using MinDI.Context.Internal;

namespace MinDI {

	public class UnityContextStart : MonoBehaviour {

		public IDIContext CreateContext() {
			BeforeCreateContext();

			IDIContext context = ContextHelper.CreateContext<IGlobalContextInitializer>(null, "global");
			context = context.Reproduce<IApplicationContextInitializer>("application");
			context = ChainUserContexts(context);
			return context;
		}

		protected virtual IDIContext ChainUserContexts(IDIContext rootContext) {
			return rootContext;
		}

		// Add some global initialization, like setting type providers here
		protected virtual void BeforeCreateContext () {
		}

		// All the global initialization happens here
		public virtual void GlobalStart(IDIContext context) {
		}

	}

}
