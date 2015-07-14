using System;

namespace MinDI.Assertions
{
	public static partial class Assert {

		public static void That (Func<bool> act, string message, IExceptionFactory exceptionFactory) {
			if (!act ()) {
				throw exceptionFactory.CreateException(message);
			}
		}

		public static void That(Func<bool> act, string message = null) {
			That (act, message, new AssertionExceptionFactory());
		}

		// Auto-generate code for All the assertions

		static void Generate() {
			// TODO

			// GenerateAssertion("NotNull", "obj != null");
			// GenerateAssertion("Null", "obj == null");
			// GenerateAssertion("AreEqual", "obj1.Equals(obj2)");
			// GenerateAssertion("AreNotEqual", "!obj1.Equals(obj2)");
		}

			
	}
}

