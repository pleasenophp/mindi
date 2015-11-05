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
		public IDictionary<string, Stack<Coroutine>> trackedCoroutines { get; set; }

		// Start one coroutine
		public Coroutine StartCoroutine(string identifier, IEnumerator routine) {
			Coroutine crt = StartCoroutine(routine);
			RegisterCoroutine(identifier, crt);
			return crt;
		}

		public Coroutine StartCoroutine(object obj, IEnumerator routine) {
			return StartCoroutine(GetObjectIdentifier(obj), routine);
		}

		public Coroutine StartCoroutines(params IEnumerator[] routines) {
			return StartCoroutines(null, null, routines);
		}

		public Coroutine StartCoroutines(Action finalCall, params IEnumerator[] routines) {
			return StartCoroutines(null, finalCall, routines);
		}

		public Coroutine StartCoroutines(object obj, params IEnumerator[] routines) {
			return StartCoroutines(GetObjectIdentifier(obj), routines);
		}

		public Coroutine StartCoroutines(string identifier, params IEnumerator[] routines) {
			return StartCoroutines(identifier, null, routines);
		}

		public Coroutine StartCoroutines(object obj, Action finalCall, params IEnumerator[] routines) {
			return StartCoroutines(GetObjectIdentifier(obj), finalCall, routines);
		}

		public Coroutine StartCoroutines(string identifier, Action finalCall, params IEnumerator[] routines) {
			Coroutine crt = StartCoroutine(RunCoroutines(identifier, finalCall, routines));
			RegisterCoroutine(identifier, crt);
			return crt;
		}

		public void StopCoroutines(object obj) {
			StopCoroutines(GetObjectIdentifier(obj));
		}

		public void StopCoroutines(string identifier) {
			Stack<Coroutine> coroutines = null;
			if (!this.trackedCoroutines.TryGetValue(identifier, out coroutines)) {
				return;
			}

			while (coroutines.Count > 0) {
				Coroutine crt = coroutines.Pop();
				StopCoroutine(crt);
			}
		}

		private void RegisterCoroutine(string identifier, Coroutine crt) {
			if (string.IsNullOrEmpty(identifier)) {
				return;
			}

			Stack<Coroutine> coroutines = null;
			if (!this.trackedCoroutines.TryGetValue(identifier, out coroutines)) {
				coroutines = new Stack<Coroutine>();
			}

			coroutines.Push(crt);
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

		private string GetObjectIdentifier(object obj) {
			UnityEngine.Object unityObject = obj as UnityEngine.Object;
			if (unityObject != null) {
				return unityObject.GetInstanceID().ToString();
			}
			else {
				return obj.GetType().FullName;
			}
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
