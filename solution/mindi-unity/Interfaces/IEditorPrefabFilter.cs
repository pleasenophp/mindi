using UnityEngine;

namespace MinDI.UnityEditor {
	public interface IEditorPrefabFilter {
		bool IsPrefab(GameObject obj);
	}
}