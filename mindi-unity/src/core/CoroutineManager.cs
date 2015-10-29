using UnityEngine;
using System.Collections;
using MinDI;
using System;

namespace MinDI.Unity {

	public class CoroutineManager : ContextMonoBehaviour, ICoroutineManager {

		public event Action onUpdate = delegate {};
		public event Action onFixedUpdate = delegate {};
		public event Action onLateUpdate = delegate {};
		public event Action onGui = delegate {};
		public event Action onDrawGizmos = delegate {};

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

		void Update() {
			onUpdate();
		}

		void FixedUpdate() {
			onFixedUpdate();
		}

		void LateUpdate() {
			onLateUpdate();
		}

		void OnGUI() {
			onGui();
		}

		void OnDrawGizmos() {
			onDrawGizmos();
		}

	}

}
