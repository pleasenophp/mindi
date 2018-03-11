using MinDI.Introspection;

namespace MinDI.Binders {
	public class MultipleBinder : BaseDIBinder {
		public MultipleBinder(IDIContext context) : base(context) {
			this.instantiationType = InstantiationType.Abstract;
		}
	}
}