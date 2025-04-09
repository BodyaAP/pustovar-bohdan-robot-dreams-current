using System;
using UnityEngine;

namespace MyLesson19
{
    namespace StateMachineSystem.ServiceLocatorSystem
    {
        [DefaultExecutionOrder(-20)]
        public abstract class GlobalMonoServiceBase : MonoServiceBase
        {
            protected override void Awake()
            {
                base.Awake();
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}