using System;

namespace MinDI {

	// Property injection attribute
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
	public sealed class ContextAssemblyAttribute : Attribute {
	}
}