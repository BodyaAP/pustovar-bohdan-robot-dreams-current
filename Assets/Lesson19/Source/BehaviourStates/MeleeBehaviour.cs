using System.Collections;
using System.Collections.Generic;
using MyLesson19.BehaviourTreeSystem;
using MyLesson19.BehaviourTreeSystem.BehaviourStates;
using MyLesson19.Dummies;
using MyLesson19.StateMachineSystem;
using MyLesson19.StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace MyLesson19
{
    public class MeleeBehaviour : BehaviourStateBase
    {
        private readonly Transform _characterTransform;
        private readonly Transform _weaponTransform;
        //private IHealthService _healthService;

        private float _time;

        public MeleeBehaviour(StateMachine stateMachine, byte stateId, EnemyController enemyController) : base(stateMachine, stateId, enemyController)
        {
            _characterTransform = enemyController.CharacterTransform;
            _weaponTransform = enemyController.WeaponTransform;
        }

        public override void Enter()
        {
            base.Enter();
            //_healthService = ServiceLocator.Instance.GetService<IHealthService>();
            _time = 0f;
        }

        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            UpdateRotation();
            Melee();
            MeleeUpdate(deltaTime);
        }

        private void Melee()
        {
            Vector3 playerPosition = enemyController.Playerdar.CurrentTarget.TargetPivot.position;
            Vector3 weaponDirection = (playerPosition - _weaponTransform.position).normalized;
            _weaponTransform.rotation = Quaternion.LookRotation(weaponDirection);
        }

        private void UpdateRotation()
        {
            Vector3 direction = Vector3.ProjectOnPlane(
                enemyController.Playerdar.CurrentTarget.TargetPivot.position - _characterTransform.position,
                Vector3.up).normalized;
            _characterTransform.rotation = Quaternion.LookRotation(direction);

            Vector3 weaponDirection = (enemyController.Playerdar.CurrentTarget.TargetPivot.position - _weaponTransform.position).normalized;
            _weaponTransform.rotation = Quaternion.LookRotation(weaponDirection);
        }

        private void MeleeUpdate(float deltaTime)
        {
            Debug.Log("Melee update");
            _time += deltaTime;

            if (_time < enemyController.Data.MeleeDelay)
            {
                return;
            }

            enemyController.HitScanGun.Melee();

            _time = 0;
            //Ray ray = new Ray(_characterTransform.position, _characterTransform.forward);
            //if (Physics.SphereCast(ray, 0.5f, out RaycastHit hitInfo, 3f))
            //{
            //    if (_healthService.GetHealth(hitInfo.collider, out Health health))
            //    {
            //        health.TakeDamage(10);
            //    }
            //}
        }

        public override void Dispose()
        {

        }
    }
}