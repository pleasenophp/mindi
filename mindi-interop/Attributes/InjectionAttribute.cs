using System;

namespace MinDI {

	// Property injection attribute
	[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	public sealed class InjectionAttribute : Attribute {
	}
}