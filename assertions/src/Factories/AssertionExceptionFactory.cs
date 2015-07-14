using System;

namespace MinDI.Assertions
{
	public class AssertionExceptionFactory : IExceptionFactory {
		#region IExceptionFactory implementation
		public Exception CreateException (string message) {
			if (string.IsNullOrEmpty (message)) {
				return new AssertionException ();
			} 
			else {
				return new AssertionException (message);
			}
		}
		#endregion
	}
}

