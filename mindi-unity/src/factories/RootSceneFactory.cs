using UnityEngine;
using System.Collections;
using MinDI;
using MinDI.Context;
using MinDI.StateObjects;
using MinDI.Introspection;
using System.Collections.Generic;

namespace MinDI.Unity {

	public class RootSceneFactory : SceneFactory {
		
		public override T Create <T>(string sceneName, bool destroyableObjects, string bindingName = null)
		{
			if (environment != ContextEnvironment.RemoteObjects) {
				throw new MindiException("RootSceneFactory can only work in the Remote objects environment");
			}

			IList<ISceneContextInitializer> initializers = ContextBuilder.Initialize<ISceneContextInitializer>(context, new SceneContextAttribute(sceneName));
			return CreateScene<T>(context, initializers, sceneName, bindingName);
		}

	}
}
