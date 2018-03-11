using System;
using MinDI.StateObjects;

namespace MinDI {
	/// <summary>
	/// Context object is an automatical memory of context on the object.
	/// Derive your MinDI objects from this class if you don't want to loose the context in some situations
	/// If you want an object with context access use this. 
	/// Usually it's a factory that should have such a privilegy.
	/// </summary>
	[Serializable]
	public class OpenContextObject : ContextObject {
		[NonSerialized] private IDIContext _contextCache;

		protected IDIContext context {
			get {
				IDIClosedContext ctx = this;
				if (!ctx.IsValid()) {
					return null;
				}

				return _contextCache ?? (_contextCache = ctx.descriptor.context);
			}
		}
	}
}