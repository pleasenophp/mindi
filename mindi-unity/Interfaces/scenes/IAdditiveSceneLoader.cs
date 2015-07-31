using UnityEngine;
using System.Collections;
using MinDI;
using System;

namespace MinDI {
	public interface IAdditiveSceneLoader : ISceneLoader
	{
		ISceneObject Unload(ISceneObject obj);
		T Unload<T>(T obj) where T:class, ISceneObject;
	}
}

