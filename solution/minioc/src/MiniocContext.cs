using System;
using minioc.context;
using minioc.misc;
using minioc.resolution.core;
using minioc.resolution.dependencies;
using MinDI;
using MinDI.StateObjects;
using MinDI.Introspection;
using MinDI.Resolution;

namespace minioc {
	public class MiniocContext : IDependencyResolver, IDIContext {
		private readonly MiniocBindings _bindings = new MiniocBindings();
		private readonly InjectionContext _injector;
		private readonly MiniocContext _parentContext;

		public string name { get; private set; }

		private readonly object locker = new object();

		public MiniocContext() : this(null, null) {
		}

		public MiniocContext(string name) : this(null, name) {
		}

		public MiniocContext(IDIContext parentContext) : this(parentContext, null) {
		}

		public MiniocContext(IDIContext parentContext, string name) {
			_parentContext = parentContext as MiniocContext;
			this.name = name;
			_injector = new DefaultInjectionContext(new ReflectionCache(), this);
		}


		/// <summary>
		/// Register a Binding (create with Bindings.ForType)
		/// </summary>
		/// <param name="binding"></param>
		public void Register(IBinding binding) {
			_bindings.Add(binding);
		}

		public T Resolve<T>(string name = null) {
			return (T) Resolve(typeof(T), name, null, false);
		}

		public T Resolve<T>(Func<IConstruction> construction, string name = null) {
			return (T) Resolve(typeof(T), name, construction, false);
		}


		public T TryResolve<T>(string name = null) {
			return (T) TryResolve(typeof(T), name, null, false);
		}

		public T TryResolve<T>(Func<IConstruction> construction, string name = null) {
			return (T) TryResolve(typeof(T), name, construction, false);
		}

		public object TryResolve(Type type, string name = null) {
			return TryResolve(type, name, null, false);
		}

		public object TryResolve(Type type, Func<IConstruction> construction, string name = null) {
			return TryResolve(type, name, construction, false);
		}

		public object Resolve(Type type, string name = null) {
			return Resolve(type, name, null, false);
		}

		public object Resolve(Type type, Func<IConstruction> construction) {
			return Resolve(type, null, construction, false);
		}

		public object Resolve(Type type, Func<IConstruction> construction, string name) {
			return Resolve(type, name, construction, false);
		}

		object IDependencyResolver.TryResolve(Type type, string name, bool omitInjectDependencies) {
			return ResolveInternal(type, name, null, omitInjectDependencies);
		}

		/// <summary>
		/// Returns the value bound to given type and with given name
		/// Throws a MiniocException if not bound
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public object Resolve(Type type, string name, Func<IConstruction> construction, bool omitInjectDependencies) {
			lock (locker) {
				object result = ResolveInternal(type, name, construction, omitInjectDependencies);
				if (result == null) {
					throw new MiniocException(string.Format("Unable to resolve instance of '{0}' named '{1}'", type, name));
				}
				return result;
			}
		}

		public object TryResolve(Type type, string name, Func<IConstruction> construction, bool omitInjectDependencies) {
			lock (locker) {
				return ResolveInternal(type, name, construction, omitInjectDependencies);
			}
		}

		public IBinding Introspect<T>(string name = null) {
			return Introspect(typeof(T), name);
		}

		public IBinding Introspect(Type type, string name = null) {
			lock (locker) {
				IBinding descriptor = _bindings.Introspect(type, name);

				if (descriptor == null && _parentContext != null) {
					return _parentContext.Introspect(type, name);
				}

				if (descriptor == null) {
					descriptor = Binding.CreateEmpty(this);
				}

				return descriptor;
			}
		}

		/// <summary>
		/// Injects dependencies on an object created outside of Minioc
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="dependencies"></param>
		public void InjectDependencies(object instance, Func<IConstruction> construction = null) {
			lock (locker) {
				InjectDependenciesInternal(instance, construction);
			}
		}

		public void RemoveBinding(IBinding binding) {
			lock (locker) {
				_bindings.Remove(binding);
			}
		}

		public IDIContext parent {
			get { return _parentContext; }
		}

		private object ResolveInternal(Type type, string name, Func<IConstruction> construction, bool omitInjectDependencies) {
			object result = _bindings.Resolve(type, name);
			if (result == null && _parentContext != null) {
				result = _parentContext.TryResolve(type, name, construction, true);
			}

			if (result == null) {
				return null;
			}

			if (!omitInjectDependencies) {
				this.InjectDependenciesInternal(result, construction);
			}
			return result;
		}

		private void InjectDependenciesInternal(object instance, Func<IConstruction> construction) {
			if (instance == null) {
				return;
			}

			// We need to call getInjectionStrategies early here so we can through exception if e.g. there are Injection attributes, but the object is not IDIClosedContext
			var strategies = _injector.getInjectionStrategies(instance);

			// Not injecting any dependencies if the object is not context object
			IDIClosedContext stateInstance = instance as IDIClosedContext;
			if (stateInstance == null || !stateInstance.IsValid()) {
				RegisterRemoteObject(instance, true);
				return;
			}

			IBinding descriptor = stateInstance.descriptor.bindingDescriptor;
			if (descriptor == null) {
				throw new MindiException("Called inject dependencies on an instance that has no binding descriptor set: " + instance);
			}

			// If this instance is concrete on another layer, we inject dependencies on its own layer only, to avoid subjectivization
			if (descriptor.instantiationType == InstantiationType.Concrete && descriptor.context != this) {
				descriptor.context.InjectDependencies(instance, construction);
				return;
			}

			if (stateInstance.descriptor.diState == DIState.NotResolved) {
				stateInstance.descriptor.diState = DIState.Resolving;
				_injector.injectDependencies(instance, strategies, construction);
				RegisterRemoteObject(instance);
				stateInstance.AfterInjection();
				stateInstance.descriptor.diState = DIState.Resolved;
			}
		}

		private void RegisterRemoteObject(object instance, bool hashOnly = false) {
			if (!RemoteObjectsHelper.IsRemoteObject(instance)) {
				return;
			}
			IRemoteObjectsRecord remoteRecord = (IRemoteObjectsRecord) this.ResolveInternal(typeof(IRemoteObjectsRecord), null, null, false);
			if (!hashOnly) {
				remoteRecord.Register(instance);
			}
			else {
				IRemoteObjectsHash hash = (IRemoteObjectsHash) this.ResolveInternal(typeof(IRemoteObjectsHash), null, null, false);
				hash.Register(instance);
			}
		}
	}
}