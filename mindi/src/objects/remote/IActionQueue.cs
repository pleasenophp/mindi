using System;

namespace MinDI.StateObjects {
	public interface IActionQueue {
		void Enqueue(Action action);
		void Process();
	}
}

