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
using MinDI.Resolution;

namespace minioc
{
	public class MiniocContext : IDependencyResolver, IDIContext {
		private MiniocBindings _bindings = new MiniocBindings ();
		private InjectionContext _injectionContext;
		private MiniocContext _parentContext;

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
			_parentContext = parentContext as MiniocContext;
			this.name = name;
			_injectionContext = new DefaultInjectionContext (new ReflectionCache (), this);
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

		public T Resolve<T> (string name = null) {
			return (T)Resolve(typeof(T), name, null, false);
		}
			
		public T Resolve<T> (IConstruction construction, string name = null) {
			return (T)Resolve(typeof(T), name, construction, false);
		}

		public object Resolve (Type type, string name=null) {
			return Resolve(type, name, null, false);
		}


		public object Resolve (Type type, IConstruction construction) {
			return Resolve(type, null, construction, false);
		}

		public object Resolve (Type type, IConstruction construction, string name) {
			return Resolve(type, name, construction, false);
		}

		object IDependencyResolver.Resolve(Type type, string name, bool omitInjectDependencies) {
			return Resolve(type, name, null, omitInjectDependencies);
		}

		object IDependencyResolver.TryResolve(Type type, string name, bool omitInjectDependencies) {
			return TryResolve(type, name, null, omitInjectDependencies);
		}
	
		/// <summary>
		/// Returns the value bound to given type and with given name
		/// Throws a MiniocException if not bound
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public object Resolve (Type type, string name, IConstruction construction, bool omitInjectDependencies)
		{
			lock (locker) {
				object result = ResolveInternal(type, name, construction, omitInjectDependencies);
				if (result == null) {
					throw new MiniocException(string.Format("Unable to resolve instance of '{0}' named '{1}'", type, name));
				}
				return result;
			}
		}

		public object TryResolve(Type type, string name, IConstruction construction, bool omitInjectDependencies) {
			lock (locker) {
				return ResolveInternal(type, name, construction, omitInjectDependencies);
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
		public void InjectDependencies (object instance, IConstruction construction = null)
		{
			lock (locker) {
				InjectDependenciesInternal(instance, construction);
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
			
		private object ResolveInternal(Type type, string name, IConstruction construction, bool omitInjectDependencies)
		{
			object result;
			if (!_bindings.tryResolve(type, name, _injectionContext, out result)) {
				if (_parentContext != null) {
					result = _parentContext.TryResolve(type, name, construction, true);
				}
			}

			if (result == null) {
				return null;
			}

			if (!omitInjectDependencies) {
				this.InjectDependenciesInternal(result, construction);
			}
			return result;
		}

		private void InjectDependenciesInternal (object instance, IConstruction construction)
		{
			if (instance == null) {
				return;
			}
				
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
				descriptor.context.InjectDependencies(instance, construction);
				return;
			}

			if (stateInstance.diState == DIState.NotResolved) {
				stateInstance.diState = DIState.Resolving;
				_injectionContext.injectDependencies(instance, construction);
				RegisterRemoteObject(instance);
				stateInstance.AfterInjection();
				stateInstance.diState = DIState.Resolved;
			}
		}
			
		private void RegisterRemoteObject(object instance, bool hashOnly = false) {
			if (RemoteObjectsHelper.IsRemoteObject(instance)) {
				IRemoteObjectsRecord remoteRecord = (IRemoteObjectsRecord)this.ResolveInternal(typeof(IRemoteObjectsRecord), null, null, false);

				if (!hashOnly) {
					remoteRecord.Register(instance);
				}
				else {
					IRemoteObjectsHash hash = (IRemoteObjectsHash)this.ResolveInternal(typeof(IRemoteObjectsHash), null, null, false);
					hash.Register(instance);
				}
			}
		}
	}
}
