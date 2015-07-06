using System;

namespace MinDI {

	public class MindiException : Exception {
	    public MindiException(string message) : base(message) {
	    }

	    public MindiException(string message, Exception innerException) : base(message, innerException) {
	    }
	}
}