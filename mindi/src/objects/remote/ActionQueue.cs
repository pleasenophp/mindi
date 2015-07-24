using System;
using System.Collections.Generic;
using UnityEngine;

namespace MinDI.StateObjects {
	public class ActionQueue : ContextObject, IActionQueue {
		
		private Queue<Action> queue;
		private object locker;

		public ActionQueue() {
			locker = new object();
			queue = new Queue<Action>();
		}

		public void Process() {
			lock (locker) {
				if (queue.Count > 0) {
					Action action = queue.Dequeue();
					action();
				}
			}
		}

		public void Enqueue(Action action) {
			lock (locker) {
				this.queue.Enqueue(action);
			}
		}

	}
}

