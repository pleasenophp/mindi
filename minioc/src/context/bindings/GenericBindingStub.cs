using System;
using minioc.misc;
using MinDI;

namespace minioc.context.bindings {
	public class GenericBindingStub<T> {
		private string _name;

		internal GenericBindingStub(string name) {
			_name = name;
		}
			

		/// <summary>
		/// Binding will return given instance when resolved
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public IBinding ImplementedByInstance(T instance) {
			return new BindingImpl(typeof(T), _name).ImplementedByInstance(instance);
		}

		/// <summary>
		/// Binding will return value returned by factory when resolved
		/// </summary>
		/// <param name="factory"></param>
		/// <returns></returns>
		public IBinding ImplementedBy(Func<T> factory) {
			return new BindingImpl(typeof(T), _name).ImplementedBy(() => factory());
		}
	}
}