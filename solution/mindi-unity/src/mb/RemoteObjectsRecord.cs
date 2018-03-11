using System;
using System.Collections.Generic;

namespace MinDI.StateObjects {
	public class RemoteObjectsRecord : RemoteObjectsRecordRoot {
		[Injection] public IRemoteObjectsDestroyer destroyer { get; set; }

		private readonly IList<object> objects;
		private readonly IDictionary<Type, IList<object>> typedObjects;

		public RemoteObjectsRecord() {
			objects = new List<object>();
			typedObjects = new Dictionary<Type, IList<object>>();
		}

		public override void Register(object obj) {
			if (obj == null) {
				return;
			}

			base.Register(obj);
			objects.Add(obj);
			RegisterTypedObject(obj);
		}

		public override void DestroyAll() {
			IRemoteObjectsHash objectsHash = context.Resolve<IRemoteObjectsHash>();
			foreach (object o in objects) {
				destroyer.Destroy(o, objectsHash);
			}
			objects.Clear();
			typedObjects.Clear();
		}

		public override void DestroyByType<T>(Func<T, bool> condition) {
			IRemoteObjectsHash objectsHash = null;

			IList<object> typedList;
			typedObjects.TryGetValue(typeof(T), out typedList);
			if (typedList == null) {
				return;
			}

			foreach (T obj in new List<object>(typedList)) {
				if (!condition(obj)) {
					continue;
				}

				if (objectsHash == null) {
					objectsHash = context.Resolve<IRemoteObjectsHash>();
				}
				typedList.Remove(obj);
				destroyer.Destroy(obj, objectsHash);
			}
		}

		private void RegisterTypedObject(object obj) {
			IList<object> typedList;
			Type type = obj.GetType();
			if (!typedObjects.TryGetValue(type, out typedList)) {
				typedList = new List<object>();
			}
			typedList.Add(obj);
			typedObjects[type] = typedList;
		}
	}
}