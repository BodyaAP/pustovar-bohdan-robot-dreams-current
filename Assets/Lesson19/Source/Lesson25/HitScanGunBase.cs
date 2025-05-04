using System;
using UnityEngine;

namespace MyLesson19
{
    namespace StateMachineSystem
    {
        public abstract class HitScanGunBase : MonoBehaviour
        {
            public event Action<Collider> OnHit;
            public event Action<RaycastHit> OnHitPrecise;
            public event Action OnShot;

            protected void InvokeHit(Collider collider)
            {
                OnHit?.Invoke(collider);
            }

            protected void InvokeHitPrecise(RaycastHit hit)
            {
                OnHitPrecise?.Invoke(hit);
            }

            protected void InvokeShot()
            {
                OnShot?.Invoke();
            }

            public abstract void Shoot();
        }
    }
}