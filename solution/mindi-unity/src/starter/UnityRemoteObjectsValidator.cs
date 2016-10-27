using MinDI.StateObjects;

namespace MinDI.StateObjects
{
	public class UnityRemoteObjectsValidator : IRemoteObjectsValidator
	{
		#region IRemoteObjectsValidator implementation
		public bool IsRemoteObject(object obj) {
			return obj is UnityEngine.Object;
		}
		#endregion
	}

}
