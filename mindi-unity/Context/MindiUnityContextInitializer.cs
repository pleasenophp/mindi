using System;
using MinDI.StateObjects;
using MinDI.Unity;

namespace MinDI.Context {
	public class MindiUnityContextInitializer : IGlobalContextInitializer {
		#region IContextInitializer implementation
		public void Initialize(IDIContext context) {
			context.s().Bind<IRemoteObjectsHash>(() => new RemoteObjectsHash());
			context.s().BindInstance<ContextEnvironment>(ContextEnvironment.RemoteObjects, "unity", (b) => b.MakeDefault());
			context.s().Bind<IRemoteObjectsRecord>(() => new RemoteObjectsRecordRoot());
			context.m().Bind<IRemoteObjectsRecord>(() => new RemoteObjectsRecord(), "factory");
			context.s().Bind<IRemoteObjectsDestroyer>(() => new RemoteObjectsDestroyer());

			context.s().Bind<RootSceneFactory>(() => new RootSceneFactory());

			context.s().BindInstance<MBLifeTime>(MBLifeTime.Permanent);

			context.m().Bind<ISceneObject>(() => new SceneObject());
			context.m().Bind<IDISceneFactory>(() => new SceneFactory());
			context.mbs().Bind<ISceneLoader, SceneLoader>();
			context.m().Bind<IAdditiveSceneLoader>(() => new AdditiveSceneLoader());

			context.mbs().Bind<ICoroutineManager, CoroutineManager>();

		}
		#endregion
		
	}
}

