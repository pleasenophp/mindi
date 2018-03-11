using MinDI.Unity;

namespace MinDI.Context.Internal {
	public class MindiUnityUserContextInitializer : IApplicationContextInitializer {
		#region IContextInitializer implementation
		public void Initialize(IDIContext context) {
			context.mbs().Bind<ICoroutineManager, CoroutineManager>();
		}
		#endregion
		
	}
}

