using System;

namespace MinDI.Assertions
{
	public class AssertionException : Exception
	{
		public const string AssertionFailedMessage = "Assertion failed !";
		public AssertionException (string message = AssertionFailedMessage) : base (message) {
		}
	}
}

