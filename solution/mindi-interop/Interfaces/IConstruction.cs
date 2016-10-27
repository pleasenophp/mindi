using System;
using System.Collections.Generic;
using minioc.resolution.dependencies;

namespace MinDI.Resolution
{
	public interface IConstruction
	{
		IDependencyResolver GetExplicitContext();
	}

}

