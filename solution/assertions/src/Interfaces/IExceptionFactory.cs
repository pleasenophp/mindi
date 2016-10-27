using System;

namespace MinDI.Assertions
{
	public interface IExceptionFactory
	{
		Exception CreateException(string message);
	}

}

