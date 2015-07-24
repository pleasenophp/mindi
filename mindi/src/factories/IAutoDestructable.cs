using System;

namespace MinDI.Factories {
	public interface IAutoDestructable {
		IDestroyingFactory factory {get; set;}
	}
}

