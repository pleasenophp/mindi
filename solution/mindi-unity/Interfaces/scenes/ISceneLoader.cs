using System;
using MinDI.Unity;

namespace MinDI {
	public interface ISceneLoader {
		void Load(string name, Action<ISceneObject> callback);
		void Load(string name, ISceneArguments arguments = null, Action<ISceneObject> callback = null);
		void Load<T>(string name, Action<T> callback) where T : class, ISceneObject;
		void Load<T>(string name, ISceneArguments arguments = null, Action<T> callback = null) where T : class, ISceneObject;
	}
}