using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace MinDI {
	[SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
	public class UnityContextStart : MonoBehaviour {
		public IDIContext CreateContext() {
			BeforeCreateContext();

			var context = ContextHelper.CreateContext<IGlobalContextInitializer>(null, "global");
			context = context.Reproduce<IApplicationContextInitializer>("application");
			context = ChainUserContexts(context);
			return context;
		}

		protected virtual IDIContext ChainUserContexts(IDIContext rootContext) {
			return rootContext;
		}

		// Add some global initialization, like setting type providers here
		protected virtual void BeforeCreateContext() {
		}

		// All the global initialization happens here
		public virtual void GlobalStart(IDIContext context) {
		}
	}
}