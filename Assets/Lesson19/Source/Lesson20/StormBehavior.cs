using System.Collections.Generic;
using BehaviourTreeSystem;
using BehaviourTreeSystem.BehaviourStates;
using MyLesson19.BehaviourTreeSystem;
using MyLesson19.BehaviourTreeSystem.BehaviourStates;
using MyLesson19.StateMachineSystem;
using MyLesson19.StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;
using UnityEngine.AI;

namespace MyLesson19
{
    namespace DefendFlag
    {
        public class StormBehavior : BehaviourStateBase
        {
            private readonly NavMeshAgent _agent;
            private readonly CharacterController _characterController;
            private readonly Transform _characterTransform;
            //private readonly IDefendFlagModeService _defendFlagModeService;

            public StormBehavior(StateMachine stateMachine, byte stateId, EnemyController enemyController) : base(
                stateMachine, stateId, enemyController)
            {
                _agent = enemyController.NavMeshAgent;
                _characterController = enemyController.CharacterController;
                _characterTransform = enemyController.CharacterTransform;
                //_defendFlagModeService = ServiceLocator.Instance.GetService<IDefendFlagModeService>();
            }

            public override void Enter()
            {
                base.Enter();

                enemyController.NavMeshAgent.speed = enemyController.Data.PatrolSpeed;
                //enemyController.NavMeshAgent.SetDestination(_defendFlagModeService.FlagPosition);
                enemyController.NavMeshAgent.SetDestination(enemyController.Playerdar.StormTarget);

                conditions = new List<IStateCondition>
                    { new BaseCondition((byte)EnemyBehaviour.Deciding, ArrivedCondition) };
            }

            protected override void OnUpdate(float deltaTime)
            {
                base.OnUpdate(deltaTime);

                Vector3 velocity = _agent.desiredVelocity;
                velocity.y = 0;
                Vector3 position = _characterTransform.position;
                _characterController.Move(velocity * (deltaTime * enemyController.Data.PatrolSpeed) + Physics.gravity);
                Vector3 newPosition = _characterTransform.position;
                Vector3 direction = newPosition - position;
                float distance = (newPosition - position).magnitude;
                _agent.nextPosition = newPosition;
                enemyController.PatrolStamina -= distance;
                direction = Vector3.ProjectOnPlane(direction, Vector3.up);
                direction.Normalize();
                if (!Mathf.Approximately(direction.magnitude, 0f))
                {
                    _characterTransform.rotation = Quaternion.LookRotation(direction);
                }
            }

            public override void Dispose()
            {
            }

            private bool ArrivedCondition()
            {
                return _agent.pathPending || _agent.remainingDistance <= _agent.stoppingDistance;
            }
        }
    }
}