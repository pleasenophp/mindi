using UnityEngine;
using System.Collections;
using MinDI;
using System;
using System.Collections.Generic;

namespace MinDI.Unity {

	public class CoroutineManager : ContextMonoBehaviour, ICoroutineManager {
		
		public event Action onUpdate = delegate {};
		public event Action onFixedUpdate = delegate {};
		public event Action onLateUpdate = delegate {};
		public event Action onGui = delegate {};
		public event Action onDrawGizmos = delegate {};

		[Injection]
		public IDictionary<string, IList<Coroutine>> trackedCoroutines { get; set; }

		// Start one coroutine
		public Coroutine StartCoroutine(string identifier, IEnumerator routine) {
			Coroutine crt = StartCoroutine(routine);
			RegisterCoroutine(identifier, crt);
			return crt;
		}

		public Coroutine StartCoroutines(params IEnumerator[] routines) {
			return StartCoroutines(null, null, routines);
		}

		public Coroutine StartCoroutines(Action finalCall, params IEnumerator[] routines) {
			return StartCoroutines(null, finalCall, routines);
		}

		public Coroutine StartCoroutines(string identifier, params IEnumerator[] routines) {
			return StartCoroutines(identifier, null, routines);
		}

		public Coroutine StartCoroutines(string identifier, Action finalCall, params IEnumerator[] routines) {
			Coroutine crt = StartCoroutine(RunCoroutines(identifier, finalCall, routines));
			RegisterCoroutine(identifier, crt);
			return crt;
		}

		public void StopCoroutines(string identifier) {
			IList<Coroutine> coroutines = null;
			if (!this.trackedCoroutines.TryGetValue(identifier, out coroutines)) {
				return;
			}

			foreach (Coroutine crt in coroutines) {
				StopCoroutine(crt);
			}

			coroutines.Clear();
		}

		private void RegisterCoroutine(string identifier, Coroutine crt) {
			if (string.IsNullOrEmpty(identifier)) {
				return;
			}

			IList<Coroutine> coroutines = null;
			if (!this.trackedCoroutines.TryGetValue(identifier, out coroutines)) {
				coroutines = new List<Coroutine>();
			}

			coroutines.Add(crt);
			this.trackedCoroutines[identifier] = coroutines;
		}

		private IEnumerator RunCoroutines(string identifier, Action finalCall, params IEnumerator[] routines) {
			foreach (IEnumerator routine in routines) {
				Coroutine crt = StartCoroutine(routine);
				RegisterCoroutine(identifier, crt);
				yield return crt;
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
