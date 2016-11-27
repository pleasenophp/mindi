using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using minioc;


using System.Reflection;
using MinDI.Context;

namespace MinDI
{
	public class ReflectionTypesProvider : IContextBuilderTypesProvider
	{
		#region IContextBuilderTypesProvider implementation
		public IList<Type> GetTypes() {
			IList<Type> result = new List<Type>();
		    Assembly executingAssembly = Assembly.GetCallingAssembly();
		    Assembly callingAssembly = Assembly.GetCallingAssembly();
		    Assembly entryAssembly = Assembly.GetEntryAssembly();

			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
				if (executingAssembly != assembly && callingAssembly != assembly && entryAssembly != assembly &&
					!assembly.IsUnityProjectAssembly() && !assembly.HasContext()) {
					continue;
				}

				try {

					foreach (Type type in assembly.GetTypes()) {
						if (type.IsClass && typeof(IContextInitializer).IsAssignableFrom(type)) {
							result.Add(type);
						}
					}
				}
				catch (Exception ex) {
					System.Diagnostics.Debug.WriteLine(string.Format("MinDI failed loading assembly {0}. The exception is: {1}", assembly.FullName, ex.Message));
					ReflectionTypeLoadException tle = ex as ReflectionTypeLoadException;
					if (tle != null) {
						foreach (var le in tle.LoaderExceptions) {
							System.Diagnostics.Debug.WriteLine(string.Format("The loader exception occured for {0}: {1}", assembly.FullName, le.Message));
						}
					}

					continue;
				}
			}

			return result;
		}
		#endregion

	}

}