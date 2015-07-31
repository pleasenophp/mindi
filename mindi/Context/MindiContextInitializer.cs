using System;
using MinDI.StateObjects;

namespace MinDI.Context {
	public class MindiContextInitializer : IGlobalContextInitializer {
		#region IContextInitializer implementation
		public void Initialize(IDIContext context) {
			context.s().BindInstance<ContextEnvironment>(ContextEnvironment.Normal);
			context.s().Bind<IActionQueue>(() => new ActionQueue());
			context.s().Bind<IRemoteObjectsHash>(() => new RemoteObjectsHash());
			context.m().Bind<IRemoteObjectsRecord>(() => new RemoteObjectsRecordStub());
		}
		#endregion
		
	}
}

