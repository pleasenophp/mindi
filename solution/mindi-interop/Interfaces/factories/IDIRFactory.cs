using MinDI.Context;

namespace MinDI {
	/// <summary>
	/// A chain factory interface to produce objects. This factory will always create new context.
	/// </summary>
	public interface IDIRFactory<T, TInitializer> : IDIFactory<T>
		where T : class where TInitializer : class, IContextInitializer {
	}

	public interface IDIRFactory<T, TInitializer1, TInitializer2> : IDIFactory<T>
		where T : class where TInitializer1 : class, IContextInitializer where TInitializer2 : class, IContextInitializer {
	}

	public interface IDIRFactory<T, TInitializer1, TInitializer2, TInitializer3> : IDIFactory<T>
		where T : class where TInitializer1 : class, IContextInitializer where TInitializer2 : class, IContextInitializer where TInitializer3 : class, IContextInitializer {
	}
}