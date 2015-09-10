using UnityEngine;
using System.Collections;
using MinDI;
using MinDI.Context;
using MinDI.StateObjects;
using MinDI.Introspection;
using System.Collections.Generic;
using System;
using MinDI.Resolution;

namespace MinDI.Unity {

	public class RootSceneFactory : SceneFactory {
		
		public override T Create <T>(string sceneName, bool destroyableObjects, string bindingName = null, 
			Action<IDIContext> customContextInitializer = null, Func<IConstruction> construction = null)
		{
			if (environment != ContextEnvironment.RemoteObjects) {
				throw new MindiException("RootSceneFactory can only work in the Remote objects environment");
			}

			IList<ISceneContextInitializer> initializers = ContextBuilder.Initialize<ISceneContextInitializer>(context, new SceneContextAttribute(sceneName));
			return CreateScene<T>(context, initializers, sceneName, bindingName, construction);
		}

	}
}
