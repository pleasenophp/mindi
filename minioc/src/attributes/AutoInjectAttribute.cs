using System;

namespace minioc.attributes {
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public sealed class AutoInjectAttribute : Attribute {
}
}