using System;

namespace minioc.resolution.dependencies {
	internal interface DependencyTarget {
		bool accept(Type type, string name);
	}
}