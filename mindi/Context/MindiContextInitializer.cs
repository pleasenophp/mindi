using System;
using MinDI.StateObjects;

namespace MinDI.Context {
	public class MindiContextInitializer : IGlobalContextInitializer {
		#region IContextInitializer implementation
		public void Initialize(IDIContext context) {
			context.s().BindInstance<ContextEnvironment>(ContextEnvironment.Normal);
			context.s().Bind<IActionQueue>(() => new ActionQueue());

			context.m().BindGeneric(typeof(IDIFactory<>), typeof(ContextFactory<>));
			context.m().BindGeneric(typeof(IDIChainFactory<,>), typeof(ContextChainFactory<,>));
		}
		#endregion
		
	}
}

