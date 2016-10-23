using System;
using MinDI.Resolution;

namespace MinDI {

	// Property injection attribute
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	public class InjectionAttribute : Attribute {
		public ResolutionOrder resolution { get; set; }
		public bool soft { get; set; }

		public InjectionAttribute() {
			resolution = ResolutionOrder.FirstExplicitThanContext;
			soft = false;
		}
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class SoftInjectionAttribute : InjectionAttribute {
		public SoftInjectionAttribute() {
			resolution = ResolutionOrder.FirstExplicitThanContext;
			soft = true;
		}
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class RequirementAttribute : InjectionAttribute {
		public RequirementAttribute() {
			resolution = ResolutionOrder.ExplicitOnly;
			soft = false;
		}
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class SoftRequirementAttribute : InjectionAttribute {
		public SoftRequirementAttribute() {
			resolution = ResolutionOrder.ExplicitOnly;
			soft = true;
		}
	}
}