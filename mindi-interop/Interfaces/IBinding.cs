using System;
using MinDI.Introspection;

namespace MinDI {
	public interface IBinding {
		BindingDescriptor descriptor {get; }

		/// <summary>
		/// Specify if the Binding uses the same instance for each 'Resolve' call or if a new instance is created each time
		/// </summary>
		/// <param name="instantiationMode"></param>
		/// <returns></returns>
		IBinding SetInstantiationMode(InstantiationMode instantiationMode);

		/// <summary>
		/// Specify the class used to create the instance 
		/// </summary>
		/// <param name="instantiator"></param>
		/// <returns></returns>
		IBinding InstanciatedBy(Instantiator instantiator);

		/// <summary>
		/// Add a dependency
		/// </summary>
		/// <param name="dependency"></param>
		/// <returns></returns>
		IBinding DependsOn(IDependency dependency);


		/// <summary>
		/// Sets the descriptor parameters for the introspection.
		/// </summary>
		/// <returns>The descriptor.</returns>
		/// <param name="context">Context.</param>
		/// <param name="instantiation">Instantiation.</param>
		/// <param name="type">Type.</param>
		/// <param name="factory">Factory.</param>
		IBinding SetDescriptor(IDIContext context, InstantiationType instantiation, BindingType type, Func<object> factory);

		/// <summary>
		/// Make this binding the default binding for the bound type
		/// </summary>
		IBinding MakeDefault();
	}
}

