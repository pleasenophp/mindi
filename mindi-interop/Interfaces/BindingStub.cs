using System;
using MinDI;

namespace MinDI {
	public interface BindingStub {
		/// <summary>
		/// Binding will create an instance of given type when resolved
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		IBinding ImplementedBy(Type type);

		/// <summary>
		/// Binding will return given instance when resolved
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		IBinding ImplementedByInstance(object instance);

		/// <summary>
		/// Binding will return value returned by factory when resolved
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		IBinding ImplementedBy(Func<object> factory);
	}
}