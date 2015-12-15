using System;
using System.Collections.Generic;

namespace MinDI {
	public interface IContextBuilderTypesProvider {
		IList<Type> GetTypes();
	}
}

