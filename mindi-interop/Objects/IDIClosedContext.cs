﻿
namespace MinDI.StateObjects {
	public interface IDIClosedContext {
		ContextDescriptor descriptor { get; }
		void AfterInjection();
		void BeforeFactoryDestruction();
	}
}

