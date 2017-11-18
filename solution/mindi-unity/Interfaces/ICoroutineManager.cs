using UnityEngine;
using System.Collections;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MinDI {
	[SuppressMessage("ReSharper", "EventNeverSubscribedTo.Global")]
	[SuppressMessage("ReSharper", "UnusedMember.Global")]
	[SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
	public interface ICoroutineManager {
		event Action onUpdate;
		event Action onFixedUpdate;
		event Action onLateUpdate;
		event Action onGui;
		event Action onDrawGizmos;
		event Action onRenderObject;
		
		// Start one coroutine
		Coroutine StartCoroutine(IEnumerator routine);
		Coroutine StartCoroutine(string identifier, IEnumerator routine);
		Coroutine StartCoroutine(object obj, IEnumerator routine);

		// Run several coroutines one by one
		Coroutine StartCoroutines(params IEnumerator[] routines);
		Coroutine StartCoroutines(Action finalCall, params IEnumerator[] routines);
		Coroutine StartCoroutines(string identifier, params IEnumerator[] routines);
		Coroutine StartCoroutines(object obj, params IEnumerator[] routines);
		Coroutine StartCoroutines(string identifier, Action finalCall, params IEnumerator[] routines);
		Coroutine StartCoroutines(object obj, Action finalCall, params IEnumerator[] routines);

		void StopCoroutine(IEnumerator routine);
		void StopCoroutine(Coroutine routine);

		// Stop all registered coroutines
		void StopCoroutines(string identifier);
		void StopCoroutines(object obj);
	}

}
