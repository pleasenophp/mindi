using System;
using System.Collections;
using MinDI.StateObjects;

namespace MinDI {

	/// <summary>
	/// Context object is an automatical memory of context on the object.
	/// Derive your MinDI objects from this class if you don't want to loose the context in some situations
	/// If you want an object with context access use this. 
	/// Usually it's a factory that should have such a privilegy.
	/// </summary>
	[Serializable]
	public class OpenContextObject : ContextObject, IDIClosedContext {
		[NonSerialized]
		private IDIContext _contextCache = null;

		protected IDIContext context {
			get {
				if (_contextCache == null) {
					_contextCache = (this as IDIClosedContext).context;
				}

				return _contextCache;
			}
		}
	}

}
