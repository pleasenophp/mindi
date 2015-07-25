using System;

namespace MinDI.StateObjects{
	public interface IAutoDestructable {
		IDestroyingFactory factory {get; set;}
	}
}

