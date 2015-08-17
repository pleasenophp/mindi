using System;
using minioc.context.bindings;
using MinDI;

namespace minioc {
	public static class Bindings {
		/// <summary>
		/// Creates a Binding for the type T. The instance can be named with the parameter 'name'
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		public static GenericBindingStub<T> ForType<T>(string name = null) {
			return new GenericBindingStub<T>(name);
		}

		/// <summary>
		/// Creates a Binding for the given type. The instance can be named with the parameter 'name'
		/// Usage discouraged
		/// </summary>
		/// <param name="type"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static BindingStub ForType(Type type, string name = null) {
			return new BindingImpl(type, name);
		}
			
	}
}