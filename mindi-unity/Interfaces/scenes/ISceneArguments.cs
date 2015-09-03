using UnityEngine;
using System.Collections;
using MinDI;
using System;
using MinDI.Resolution;

namespace MinDI {
	public interface ISceneArguments
	{
		IConstruction CreateConstruction();
		void PopulateContext(IDIContext context);
	}
}

