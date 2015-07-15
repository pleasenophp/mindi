using System;
using System.Collections.Generic;
using minioc.context;
using minioc.context.bindings;
using minioc.misc;
using minioc.resolution.core;
using minioc.resolution.dependencies;
using MinDI;
// using UnityEngine;
using MinDI.StateObjects;

namespace minioc
{
	public class MiniocContext : DependencyResolver, IDIContext {
		private MiniocBindings _bindings = new MiniocBindings ();
		private InjectionContext _injectionContext;
		private IDIContext _parentContext;

		~MiniocContext ()
		{
			// Debug.LogWarning ("DESTROYED CONTEXT: " + name);
		}

		public MiniocContext () : this(null, null)
		{
		}

		public MiniocContext (string name) : this(null, name)
		{
		}

		public MiniocContext (IDIContext parentContext) : this(parentContext, null)
		{
		}

		public MiniocContext (IDIContext parentContext, string name)
		{
			_parentContext = parentContext;
			this.name = name;
			_injectionContext = new DefaultInjectionContext (_bindings, new ReflectionCache (), this);
			// Debug.LogWarning ("CREATED CONTEXT: " + name);
		}


		/// <summary>
		/// Register a Binding (create with Bindings.ForType)
		/// </summary>
		/// <param name="binding"></param>
		public void Register (IBinding binding)
		{
			BindingImpl impl = (BindingImpl)binding;
			impl.verifyIntegrity ();
			_bindings.add (impl);
		}


		/// <summary>
		/// Returns the value bound to type T
		/// Throws a MiniocException if not bound
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T Resolve<T> (bool omitInjectDependencies = false)
		{
			return (T)Resolve (typeof(T), omitInjectDependencies);
		}

		/// <summary>
		/// Returns the value bound to type T and with given name
		/// Throws a MiniocException if not bound
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T Resolve<T> (string name, bool omitInjectDependencies = false)
		{
			if (string.IsNullOrEmpty(name)) {
				return (T)Resolve (typeof(T), omitInjectDependencies);
			}

			return (T)Resolve (typeof(T), name, omitInjectDependencies);
		}

		/// <summary>
		/// Returns the value bound to given type
		/// Throws a MiniocException if not bound
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public object Resolve (Type type, bool omitInjectDependencies = false)
		{
			try {
				if (_parentContext != null) {
					object result;
					if (_bindings.tryResolveDefault (type, _injectionContext, out result)) {
						if (!omitInjectDependencies) {
							this.InjectDependencies (result);
						}
						return result;
					} else {
						result = _parentContext.Resolve (type, true);
						if (!omitInjectDependencies) {
							this.InjectDependencies(result);
						}
						return result;
					}
				} else {
					object result = _bindings.resolveDefault (type, _injectionContext);
					if (!omitInjectDependencies) {
						this.InjectDependencies(result);
					}
					return result;
				}
			} catch (MiniocException e) {
				throw new MiniocException (string.Format ("Unable to resolve instance of '{0}'", type), e);
			}
		}

		/// <summary>
		/// Returns the value bound to given type and with given name
		/// Throws a MiniocException if not bound
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public object Resolve (Type type, string name, bool omitInjectDependencies = false)
		{
			if (string.IsNullOrEmpty(name)) {
				return Resolve(type, omitInjectDependencies);
			}

			try {
				if (_parentContext != null) {
					object result;
					if (_bindings.tryResolve (type, name, _injectionContext, out result)) {
						if (!omitInjectDependencies) {
							this.InjectDependencies(result);
						}
						return result;
					} else {
						result = _parentContext.Resolve (type, name, true);
						if (!omitInjectDependencies) {
							this.InjectDependencies(result);
						}
						return result;
					}
				} else {
					object result = _bindings.resolve (type, name, _injectionContext);
					if (!omitInjectDependencies) {
						this.InjectDependencies(result);
					}
					return result;
				}
			} catch (MiniocException e) {
				throw new MiniocException (string.Format ("Unable to resolve instance of '{0}' named '{1}'", type, name), e);
			}
		}

		/// <summary>
		/// Injects dependencies on an object created outside of Minioc
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="dependencies"></param>
		public void InjectDependencies (object instance, IList<IDependency> dependencies = null)
		{
			IDIStateObject stateInstance = instance as IDIStateObject;
			if (stateInstance == null) {
				_injectionContext.injectDependencies (instance, dependencies);
			}
			else {
				if (stateInstance.diState == DIState.NotResolved) {
					stateInstance.diState = DIState.Resolving;
					_injectionContext.injectDependencies (instance, dependencies);
					stateInstance.diState = DIState.Resolved;
				}
			}
		}

		public void RemoveBinding (IBinding binding)
		{
			_bindings.remove (binding);
		}

		public string name { get; private set;}

		public IDIContext parent {
			get {
				return _parentContext;
			}
		}
	}
}
