using System;
using MinDI.Unity;
using MinDI.Resolution;

namespace MinDI {
	
	public abstract class SceneArguments : ISceneArguments {
		public virtual IConstruction CreateConstruction() {
			return Construction.Empty();
		}

		public virtual void PopulateContext(IDIContext context) {
		}
	}
}

