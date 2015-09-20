using System;
using MinDI.Introspection;

namespace MinDI.StateObjects {
	public interface IDIClosedContext {
		IDIContext context { get; }
		IBinding bindingDescriptor { get; set;}

		IDestroyingFactory factory {get; set;}

		DIState diState {get; set;}
		void AfterInjection();
		void BeforeFactoryDestruction();
	}
}

