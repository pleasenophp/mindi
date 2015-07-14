using System;

namespace MinDI {
	public interface Instantiator {
		object CreateInstance(Type type);

		bool AllowConstructorInjection();
	}
}