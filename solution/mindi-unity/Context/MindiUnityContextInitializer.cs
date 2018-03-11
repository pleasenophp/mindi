using MinDI.StateObjects;
using MinDI.Unity;

namespace MinDI.Context.Internal {
	public class MindiUnityContextInitializer : IGlobalContextInitializer {
		#region IContextInitializer implementation
		public void Initialize(IDIContext context) {
			context.s().Bind<IRemoteObjectsHash>(() => new RemoteObjectsHash());
			context.s().BindInstance<ContextEnvironment>(ContextEnvironment.RemoteObjects, "unity", true);
			context.s().BindInstance<MBLifeTime>(MBLifeTime.Permanent);
			context.s().Bind<IRemoteObjectsDestroyer>(() => new RemoteObjectsDestroyer());

			context.s().Bind<IRemoteObjectsRecord>(() => new RemoteObjectsRecordRoot());
			context.m().Bind<IRemoteObjectsRecord>(() => new RemoteObjectsRecord(), "factory");

			context.m().Bind<RootSceneFactory>(() => new RootSceneFactory());
			context.m().Bind<ISceneObject>(() => new SceneObject());
			context.m().Bind<IDISceneFactory>(() => new SceneFactory());
		}
		#endregion
		
	}
}

