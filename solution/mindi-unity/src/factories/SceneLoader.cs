using UnityEngine;
using System.Collections;
using MinDI;
using System;

namespace MinDI.Unity {

	public abstract class SceneLoader : ContextMonoBehaviour, ISceneLoader
	{
		public void Load (string name, Action<ISceneObject> callback)
		{
			Load<ISceneObject> (name, null, callback);
		}

		public void Load (string name, ISceneArguments arguments = null, Action<ISceneObject> callback = null)
		{
			Load<ISceneObject> (name, arguments, callback);
		}

		public void Load<T> (string name, Action<T> callback) where T : class, ISceneObject
		{
			Load<T> (name, null, callback);
		}

		public abstract void Load<T> (string name, ISceneArguments arguments = null, Action<T> callback = null) where T : class, ISceneObject;
	}
}
