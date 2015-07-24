using System;
using MinDI.StateObjects;

namespace MinDI.Context {
	public class MindiContextInitializer : IGlobalContextInitializer {
		#region IContextInitializer implementation
		public void Initialize(IDIContext context) {
			var bind = context.CreateBindHelper();
			bind.singleton.BindInstance<ContextEnvironment>(ContextEnvironment.Normal);
			bind.singleton.Bind<IActionQueue>(() => new ActionQueue());
		}
		#endregion
		
	}
}

