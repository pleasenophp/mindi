using System;
using UnityEngine.SceneManagement;

namespace MinDI.Unity
{
	public static class SceneHelper
	{
		public static string GetLoadedLevelName ()
		{
			Scene scene = SceneManager.GetActiveScene ();
			return scene.name;
		}
		
	}
}

