using UnityEngine;
using System.Collections;
using System;

namespace MinDI {

	public interface ICoroutineManager {
		event Action onUpdate;
		event Action onFixedUpdate;
		event Action onLateUpdate;
		event Action onGui;
		event Action onDrawGizmos;

		// Start one coroutine
		Coroutine StartCoroutine(IEnumerator routine);
		Coroutine StartCoroutine(string identifier, IEnumerator routine);

		// Run several coroutines one by one
		Coroutine StartCoroutines(params IEnumerator[] routines);
		Coroutine StartCoroutines(Action finalCall, params IEnumerator[] routines);
		Coroutine StartCoroutines(string identifier, params IEnumerator[] routines);
		Coroutine StartCoroutines(string identifier, Action finalCall, params IEnumerator[] routines);

		void StopCoroutine(IEnumerator routine);
		void StopCoroutine(Coroutine routine);

		// Stop all registered coroutines
		void StopCoroutines(string identifier);
	}

}
