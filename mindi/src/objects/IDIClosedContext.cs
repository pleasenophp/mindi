﻿using System;

namespace MinDI.StateObjects {
	public interface IDIClosedContext {
		IDIContext context { get; }

		IDIContext stCreatorContext { get; set; }
	}
}

