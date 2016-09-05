using System;
using MinDI.StateObjects;
using MinDI.Unity;

namespace MinDI.Context.Internal {
	public class MindiUnitySceneLoadersContextInitializer : IApplicationContextInitializer {
		#region IContextInitializer implementation
		public void Initialize(IDIContext context) {
			context.mbs().Bind<ISceneLoader, Unity5SceneLoader> ();
			context.m().Bind<IAdditiveSceneLoader> (() => new Unity5AdditiveSceneLoader ());
		}
		#endregion
		
	}
}

