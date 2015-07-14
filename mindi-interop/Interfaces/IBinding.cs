using System;

namespace MinDI {
	public interface IBinding {
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
		/// Make this binding the default binding for the bound type
		/// </summary>
		IBinding MakeDefault();
	}
}

