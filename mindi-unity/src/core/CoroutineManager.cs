using UnityEngine;
using System.Collections;
using MinDI;
using System;

namespace MinDI.Unity {

	public class CoroutineManager : ContextMonoBehaviour, ICoroutineManager {

		public Coroutine StartCoroutines(params IEnumerator[] routines) {
			return StartCoroutines(null, routines);
		}

		public Coroutine StartCoroutines(Action finalCall, params IEnumerator[] routines) {
			return StartCoroutine(RunCoroutines(finalCall, routines));
		}

		private IEnumerator RunCoroutines(Action finalCall, params IEnumerator[] routines) {
			foreach (IEnumerator routine in routines) {
				yield return StartCoroutine(routine);
			}
			if (finalCall != null) {
				finalCall();
			}
			yield break;
		}

	}

}
