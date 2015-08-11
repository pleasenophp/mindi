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
using MinDI.Introspection;

namespace minioc
{
	public class MiniocContext : DependencyResolver, IDIContext {
		private MiniocBindings _bindings = new MiniocBindings ();
		private InjectionContext _injectionContext;
		private IDIContext _parentContext;

		private object locker = new object();

		/*
		~MiniocContext ()
		{
			Debug.LogWarning ("DESTROYED CONTEXT: " + name);
		}
		*/

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
			return (T)Resolve (typeof(T), null, omitInjectDependencies);
		}

		/// <summary>
		/// Returns the value bound to type T and with given name
		/// Throws a MiniocException if not bound
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T Resolve<T> (string name, bool omitInjectDependencies = false)
		{
			return (T)Resolve (typeof(T), name, omitInjectDependencies);
		}
			
		/// <summary>
		/// Returns the value bound to given type and with given name
		/// Throws a MiniocException if not bound
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public object Resolve (Type type, string name=null, bool omitInjectDependencies = false)
		{
			lock (locker) {
				object result = ResolveInternal(type, name, omitInjectDependencies);
				if (result == null) {
					throw new MiniocException(string.Format("Unable to resolve instance of '{0}' named '{1}'", type, name));
				}
				return result;
			}
		}

		public BindingDescriptor Introspect<T>(string name=null) {
			return Introspect(typeof(T), name);
		}

		public BindingDescriptor Introspect(Type type, string name=null) {
			lock (locker) {

				BindingDescriptor descriptor = _bindings.introspect(type, name);

				if (descriptor == null && _parentContext != null) {
					return _parentContext.Introspect(type, name);
				}

				if (descriptor == null) {
					descriptor = new BindingDescriptor();
				}
	
				return descriptor;
			}
		}

		/// <summary>
		/// Injects dependencies on an object created outside of Minioc
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="dependencies"></param>
		public void InjectDependencies (object instance, IList<IDependency> dependencies = null)
		{
			lock (locker) {
				InjectDependenciesInternal(instance, dependencies);
			}
		}
			
		public void RemoveBinding (IBinding binding)
		{
			lock (locker) {
				_bindings.remove(binding);
			}
		}

		public string name { get; private set;}

		public IDIContext parent {
			get {
				return _parentContext;
			}
		}

		private object ResolveInternal(Type type, string name=null, bool omitInjectDependencies = false)
		{
			try {
				if (_parentContext != null) {
					object result;

					if (_bindings.tryResolve(type, name, _injectionContext, out result)) {
						if (!omitInjectDependencies) {
							this.InjectDependenciesInternal(result);
						}
						return result;
					}
					else {
						result = _parentContext.Resolve(type, name, true);

						if (!omitInjectDependencies) {
							this.InjectDependenciesInternal(result);
						}
						return result;
					}
				}
				else {
					object result = _bindings.resolve(type, name, _injectionContext);
					if (!omitInjectDependencies) {
						this.InjectDependenciesInternal(result);
					}
					return result;
				}
			}
			catch (MiniocException e) {
				throw new MiniocException(string.Format("Unable to resolve instance of '{0}' named '{1}'", type, name), e);
			}

		}

		private void InjectDependenciesInternal (object instance, IList<IDependency> dependencies = null)
		{
			if (instance == null) {
				return;
			}

			// TODO - if it's not context mono behaviour, can still allow to use it by adding a state object for this MB together when instantiating.
			// Then just query for the state object that is IDIClosedContext

			// Not injecting any dependencies if the object is not context object
			IDIClosedContext stateInstance = instance as IDIClosedContext;
			if (stateInstance == null) {
				RegisterRemoteObject(instance, true);
				return;
			}

			BindingDescriptor descriptor = stateInstance.bindingDescriptor;
			if (descriptor == null) {
				throw new MindiException("Called inject dependencies on an instance that has no binding descriptor set: "+instance);
			}

			// If this instance is concrete on another layer, we inject dependencies on its own layer only, to avoid subjectivization
			if (descriptor.instantiationType == InstantiationType.Concrete && descriptor.context != this) {
				descriptor.context.InjectDependencies(instance, dependencies);
				return;
			}

			if (stateInstance.diState == DIState.NotResolved) {
				stateInstance.diState = DIState.Resolving;
				_injectionContext.injectDependencies(instance, dependencies);
				RegisterRemoteObject(instance);
				stateInstance.AfterInjection();
				stateInstance.diState = DIState.Resolved;
			}
		}
			
		private void RegisterRemoteObject(object instance, bool hashOnly = false) {
			if (RemoteObjectsHelper.IsRemoteObject(instance)) {
				IRemoteObjectsRecord remoteRecord = (IRemoteObjectsRecord)this.ResolveInternal(typeof(IRemoteObjectsRecord), null, false);

				if (!hashOnly) {
					remoteRecord.Register(instance);
				}
				else {
					IRemoteObjectsHash hash = (IRemoteObjectsHash)this.ResolveInternal(typeof(IRemoteObjectsHash), null, false);
					hash.Register(instance);
				}
			}
		}
	}
}
