using System;

namespace minioc.attributes {
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class InjectionPropertyAttribute : Attribute {
}
}