using UnityEngine;
using System.Collections;
using MinDI;
using System;

namespace MinDI {

	public interface ICoroutineManager {
		// Start one coroutine
		Coroutine StartCoroutine(IEnumerator routine);

		// Run several coroutines one by one
		Coroutine StartCoroutines(params IEnumerator[] routines);
		Coroutine StartCoroutines(Action finalCall, params IEnumerator[] routines);

		void StopCoroutine(IEnumerator routine);
		void StopCoroutine(Coroutine routine);
	}

}
