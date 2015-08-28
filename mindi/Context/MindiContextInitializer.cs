using System;
using MinDI.StateObjects;
using MinDI.Resolution;

namespace MinDI.Context.Internal {
	public class MindiContextInitializer : IGlobalContextInitializer {
		#region IContextInitializer implementation
		public void Initialize(IDIContext context) {
			context.s().BindInstance<ContextEnvironment>(ContextEnvironment.Normal);
			context.s().Bind<IActionQueue>(() => new ActionQueue());

			context.m().BindGeneric(typeof(IDIFactory<>), typeof(ContextFactory<>));
			context.m().BindGeneric(typeof(IDIRFactory<,>), typeof(ReproduceContextFactory<,>));
			context.m().BindGeneric(typeof(IDIRFactory<,,>), typeof(ReproduceContextFactory<,,>));
			context.m().BindGeneric(typeof(IDIRFactory<,,,>), typeof(ReproduceContextFactory<,,,>));

			context.m().BindGeneric(typeof(IDynamicInjection<>), typeof(DynamicResolver<>));
			context.m().BindGeneric(typeof(ISoftDynamicInjection<>), typeof(SoftDynamicResolver<>));
		}
		#endregion
		
	}
}

