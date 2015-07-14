using System;

namespace MinDI {
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public sealed class AutoInjectAttribute : Attribute {
	}
}