using System;
using UnityEngine;
using minioc.resolution.instantiator;
using Object = UnityEngine.Object;

namespace minioc.unity {
public class PrefabInstantiator : Instantiator {
    private Transform _parent;
    private Component _prefab;

    public PrefabInstantiator(Component prefab, Transform parent = null) {
        _parent = parent;
        _prefab = prefab;
    }

    public object CreateInstance(Type type) {
        if (_prefab.GetType() != type) {
            throw new Exception("Invalid type");
        }
        Component component = (Component)Object.Instantiate(_prefab);
        if (_parent != null) {
            component.transform.parent = _parent;
        }
        return component;
    }

    public bool AllowConstructorInjection() {
        return false;
    }
}
}