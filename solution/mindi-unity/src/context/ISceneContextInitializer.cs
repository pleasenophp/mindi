using MinDI.Context;

namespace MinDI {
	public interface ISceneContextInitializer : IContextInitializer {
		void AutoInstantiate(IDIContext context);
	}
}