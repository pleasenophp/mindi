using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Reflection;
using MinDI.Context;
using MinDI.Helpers;

namespace MinDI
{
	public class ReflectionTypesProvider : IContextBuilderTypesProvider
	{
		#region IContextBuilderTypesProvider implementation
		public IList<Type> GetTypes() {
		    PreloadAssemblies();

		    // Load types
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
					var tle = ex as ReflectionTypeLoadException;
				    if (tle == null) continue;
				    foreach (Exception le in tle.LoaderExceptions) {
				        System.Diagnostics.Debug.WriteLine(string.Format("The loader exception occured for {0}: {1}", assembly.FullName, le.Message));
				    }
				}
			}

			return result;
		}

	    private static void PreloadAssemblies()
	    {
	        var files = FileHelper.AllFilesInApplicationFolder().Where(f => f.Extension == ".dll");
	        foreach (FileInfo f in files)
	        {
	            Assembly.LoadFrom(f.FullName);
	        }
	    }

	    #endregion

	}

}