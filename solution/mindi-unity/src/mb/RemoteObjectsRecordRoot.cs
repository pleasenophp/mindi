using System;
using UnityEngine;

namespace MinDI.StateObjects {
	public class RemoteObjectsRecordRoot : OpenContextObject, IRemoteObjectsRecord {
		#region IRemoteObjectsRecord implementation

		private MBLifeTime mbLifetime;

		protected override void OnInjected() {
			mbLifetime = context.Resolve<MBLifeTime>();
		}

		public virtual void Register(object obj) {
			if (mbLifetime != MBLifeTime.Permanent) {
				return;
			}
			var mb = obj as MonoBehaviour;
			if (mb != null) {
				UnityEngine.Object.DontDestroyOnLoad(mb.gameObject);
			}
			var go = obj as GameObject;
			if (go != null) {
				UnityEngine.Object.DontDestroyOnLoad(go);
			}
		}

		public virtual void DestroyByType<T>(Func<T, bool> condition) where T : class {
		}

		public virtual void DestroyAll() {
		}

		#endregion
	}
}