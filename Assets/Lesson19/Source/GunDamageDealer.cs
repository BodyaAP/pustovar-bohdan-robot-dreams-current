using System;
using MyLesson19.Dummies;
using MyLesson19.StateMachineSystem;
using MyLesson19.StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace MyLesson19
{
    namespace BehaviourTreeSystem
    {
        public class GunDamageDealer : MonoBehaviour
        {
            public event Action<int> OnHit;

            [SerializeField] private HitScanGun _gun;
            //[SerializeField] private int _damage;

            [SerializeField] private WeaponData _data;

            public HitScanGun Gun => _gun;

            private IHealthService _healthService;

            private void Start()
            {
                _healthService = ServiceLocator.Instance.GetService<IHealthService>();

                _gun.OnHit += GunHitHandler;
            }

            private void GunHitHandler(Collider collider)
            {
                if (_healthService.GetHealth(collider, out Health health))
                    health.TakeDamage(_data.Damage);
                OnHit?.Invoke(health ? 1 : 0);
            }
        }
    }
}