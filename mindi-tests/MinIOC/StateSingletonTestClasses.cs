using System;
using System.Collections;
using minioc;
using minioc.context.bindings;
using minioc.resolution.instantiator;
using UnityEngine;
using MinDI;


namespace MinDI.Tests.MinIOC {
	public interface IAdvancedRead {
		string tag { get; }
	}

	public interface IAdvancedWrite {
		string tag { set; }
	}

	public class AdvancedModel : IAdvancedRead, IAdvancedWrite {
		public string tag { get; set; }
	}

	public interface IDependencyTest {
		IDIContext contextAccess { get; }
	}

	public class DependencyTest : OpenContextObject, IDependencyTest {
		public IDIContext contextAccess {
			get {
				return this.context;
			}
		}
	}
}