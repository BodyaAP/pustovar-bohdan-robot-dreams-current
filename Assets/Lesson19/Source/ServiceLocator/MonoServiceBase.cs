using System;
using MyLesson19.StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace MyLesson19
{
    namespace StateMachineSystem.ServiceLocatorSystem
    {
        [DefaultExecutionOrder(-10)]
        public abstract class MonoServiceBase : MonoBehaviour, IService
        {
            public abstract Type Type { get; }

            protected virtual void Awake()
            {
                ServiceLocator.Instance.AddService(this);
            }

            protected virtual void OnDestroy()
            {
                ServiceLocator.Instance.RemoveService(this);
            }
        }
    }
}