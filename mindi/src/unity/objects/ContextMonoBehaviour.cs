using System;
using System.Collections;
using minioc;
using minioc.attributes;
using minioc.context.bindings;
using minioc.resolution.instantiator;
using minioc.MinDI;


namespace MinDI.Objects {

	/// <summary>
	/// Context object is an automatical memory of context on the object.
	/// Derive your MinDI objects from this class if you don't want to loose the context in some situations
	/// At the same time, this makes context inaccessible to an ordinary object. 
	/// If you want an object with context access use PublicContextObject instead. Usually it's a factory that should have such a privilegy.
	/// </summary>
	public class ContextMonoBehaviour : DIStateMonoBehaviour
	{
	#pragma warning disable 414
		[NonSerialized]
		private MiniocContext _context;
	#pragma warning restore 414

		[InjectionProperty]
		public MiniocContext __context {
			set {
				if (_context != null) {
					throw new MindiException("Attempt to alter context on ContextObject");
				}
				_context = value;
			}
		}
	}
}
