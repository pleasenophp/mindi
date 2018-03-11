using System;
using System.Collections.Generic;

namespace MinDI.StateObjects {
	public class ActionQueue : ContextObject, IActionQueue {
		
		private readonly Queue<Action> queue;
		private readonly object locker;

		public ActionQueue() {
			locker = new object();
			queue = new Queue<Action>();
		}

		public void Process() {
			lock (locker) {
				if (queue.Count <= 0) {
					return;
				}
				var action = queue.Dequeue();
				action();
			}
		}

		public void Enqueue(Action action) {
			lock (locker) {
				this.queue.Enqueue(action);
			}
		}

	}
}

