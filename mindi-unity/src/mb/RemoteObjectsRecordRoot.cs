using System;
using System.Collections.Generic;
using UnityEngine;
using MinDI.StateObjects;

namespace MinDI.StateObjects {

	public class RemoteObjectsRecordRoot : OpenContextObject, IRemoteObjectsRecord {
		#region IRemoteObjectsRecord implementation

		private MBLifeTime mbLifetime;

		protected override void OnInjected() {
			mbLifetime = context.Resolve<MBLifeTime>();
		}

		public virtual void Register(object obj) {
			if (mbLifetime == MBLifeTime.Permanent) {
				MonoBehaviour mb = obj as MonoBehaviour;
				if (mb != null) {
					UnityEngine.Object.DontDestroyOnLoad(mb.gameObject);
				}

				GameObject go = obj as GameObject;
				if (go != null) {
					UnityEngine.Object.DontDestroyOnLoad(go);
				}
			}
		}

		public virtual void DestroyAll() {
		}

		#endregion


	}
}

