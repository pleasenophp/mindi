using System;
using MinDI;

namespace minioc.resolution.dependencies {
	public interface DependencyStub {
		/// <summary>
		/// Will resolve Dependency using the Binding of the parameter type and with given name
		/// </summary>
		/// <param name="instanceName"></param>
		/// <returns></returns>
		IDependency ResolveByName(string instanceName);

		/// <summary>
		/// Will resolve Dependency using the given value
		/// </summary>
		/// <param name="instanceName"></param>
		/// <returns></returns>
		IDependency ResolveByValue(object value);


		/// <summary>
		/// Will resolve Dependency using the given factory (will be call only when the Dependency is resolved
		/// </summary>
		/// <param name="instanceName"></param>
		/// <returns></returns>
		IDependency ResolveByFactory(Func<object> factory);
	}
}