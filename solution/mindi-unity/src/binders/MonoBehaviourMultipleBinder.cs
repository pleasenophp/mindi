namespace MinDI.Binders {
	public class MonoBehaviourMultipleBinder : MonoBehaviourBinder {
		public MonoBehaviourMultipleBinder(IDIContext context) : base(context, InstantiationMode.MULTIPLE) {
		}
	}
}