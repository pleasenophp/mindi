
namespace MinDI
{
	public interface IDynamicInjection<T>
	{
		T Resolve(string name);
	}

	public interface ISoftDynamicInjection<T> : IDynamicInjection<T> {}
}
