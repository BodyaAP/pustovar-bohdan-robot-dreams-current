using System;
using UnityEngine;

namespace MyLesson19
{
    namespace StateMachineSystem
    {
        public abstract class GrenadeSpawnerBase : MonoBehaviour
        {
            public event Action<Grenade> OnGrenadeSpawned;

            protected void InvokeOnGrenadeSpawned(Grenade grenade) => OnGrenadeSpawned?.Invoke(grenade);
        }
    }
}