using System;

namespace MinDI.StateObjects {
	public interface IDIClosedContext {
		IDIContext context { get; }

		// TODO - consider just having context lookup on where the thingie was created
		// Also consider to be able to find the direct factory of the class in the context for rebinding
		IDIContext stCreatorContext { get; set; }
	}
}

