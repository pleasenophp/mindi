using UnityEngine;
using System.Collections;
using MinDI;
using System;

namespace MinDI {
	public interface ISceneLoader
	{
		void Load(string name, Action<ISceneObject> callback = null);
		void Load<T>(string name, Action<T> callback = null) where T:class, ISceneObject;
	}
}

