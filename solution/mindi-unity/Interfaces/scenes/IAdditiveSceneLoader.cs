namespace MinDI {
	public interface IAdditiveSceneLoader : ISceneLoader {
		ISceneObject Unload(ISceneObject obj);
		T Unload<T>(T obj) where T : class, ISceneObject;
	}
}