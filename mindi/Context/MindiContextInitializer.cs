using System;
using MinDI.StateObjects;
using MinDI.Resolution;
using System.Collections.Generic;

namespace MinDI.Context.Internal {
	public class MindiContextInitializer : IGlobalContextInitializer {
		#region IContextInitializer implementation
		public void Initialize(IDIContext context) {
			// Generic collections
			context.m().BindGeneric(typeof(IList<>), typeof(List<>));
			context.m().BindGeneric(typeof(HashSet<>), typeof(HashSet<>));
			context.m().BindGeneric(typeof(IDictionary<,>), typeof(Dictionary<,>));

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

