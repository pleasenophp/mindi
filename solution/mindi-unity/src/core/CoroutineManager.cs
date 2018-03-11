using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace MinDI.Unity {

	public class CoroutineManager : ContextMonoBehaviour, ICoroutineManager {
		
		public event Action onUpdate = delegate {};
		public event Action onFixedUpdate = delegate {};
		public event Action onLateUpdate = delegate {};
		public event Action onGui = delegate {};
		public event Action onDrawGizmos = delegate {};
		public event Action onRenderObject = delegate {};

		[Injection] public IDictionary<string, Stack<Coroutine>> trackedCoroutines { get; set; }

		// Start one coroutine
		public Coroutine StartCoroutine(string identifier, IEnumerator routine) {
			var crt = StartCoroutine(routine);
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
			var crt = StartCoroutine(RunCoroutines(identifier, finalCall, routines));
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
				var crt = coroutines.Pop();
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

		private static string GetObjectIdentifier(object obj) {
			var unityObject = obj as UnityEngine.Object;
			return unityObject != null ? unityObject.GetInstanceID().ToString() : obj.GetType().FullName;
		}

		private void Update() {
			onUpdate();
		}

		private void FixedUpdate() {
			onFixedUpdate();
		}

		private void LateUpdate() {
			onLateUpdate();
		}

		private void OnGUI() {
			onGui();
		}

		private void OnDrawGizmos() {
			onDrawGizmos();
		}

		private void OnRenderObject() {
			onRenderObject();
		}
	}

}
