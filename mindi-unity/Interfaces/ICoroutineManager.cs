using UnityEngine;
using System.Collections;
using MinDI;

namespace MinDI {

	public interface ICoroutineManager {

		Coroutine StartCoroutine(IEnumerator routine);
		void StopCoroutine(IEnumerator routine);
		void StopCoroutine(Coroutine routine);
	}

}
