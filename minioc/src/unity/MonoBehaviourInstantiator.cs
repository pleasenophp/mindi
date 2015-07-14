using System;
using UnityEngine;
using minioc.resolution.instantiator;
using MinDI;

namespace minioc.unity {
public class MonoBehaviourInstantiator : Instantiator {
    private GameObject _gameObject;

    public MonoBehaviourInstantiator(GameObject gameObject) {
        _gameObject = gameObject;
    }

    public object CreateInstance(Type type) {
        return _gameObject.AddComponent(type);
    }

    public bool AllowConstructorInjection() {
        return false;
    }
}
}
