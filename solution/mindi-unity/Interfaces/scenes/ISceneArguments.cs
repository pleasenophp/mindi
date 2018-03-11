using MinDI.Resolution;

namespace MinDI.Unity {
	public interface ISceneArguments {
		IConstruction CreateConstruction();
		void PopulateContext(IDIContext context);
	}
}