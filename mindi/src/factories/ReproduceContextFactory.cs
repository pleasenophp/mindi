using System;
using System.Collections;
using minioc;


using MinDI.Context;

namespace MinDI {

	/// <summary>
	/// A factory that chains context when creating a new object
	/// </summary>
	public class ReproduceContextFactory<T, TInitializer> : ContextFactory<T>, IDIRFactory<T, TInitializer> 
	where T:class where TInitializer:class, IContextInitializer
	{
		protected override bool ForceNewContext() {
			return true;
		}

		protected override void InitNewContext(IDIContext context) {
			context.Initialize<TInitializer>();
		}
	}

	public class ReproduceContextFactory<T, TInitializer1, TInitializer2> : ContextFactory<T>, IDIRFactory<T, TInitializer1, TInitializer2> 
		where T:class where TInitializer1:class, IContextInitializer where TInitializer2:class, IContextInitializer
	{
		protected override bool ForceNewContext() {
			return true;
		}

		protected override void InitNewContext(IDIContext context) {
			context.Initialize<TInitializer1>();
			context.Initialize<TInitializer2>();
		}
	}

	public class ReproduceContextFactory<T, TInitializer1, TInitializer2, TInitializer3> : ContextFactory<T>, IDIRFactory<T, TInitializer1, TInitializer2, TInitializer3> 
		where T:class where TInitializer1:class, IContextInitializer where TInitializer2:class, IContextInitializer where TInitializer3:class, IContextInitializer
	{
		protected override bool ForceNewContext() {
			return true;
		}

		protected override void InitNewContext(IDIContext context) {
			context.Initialize<TInitializer1>();
			context.Initialize<TInitializer2>();
			context.Initialize<TInitializer3>();
		}
	}

}