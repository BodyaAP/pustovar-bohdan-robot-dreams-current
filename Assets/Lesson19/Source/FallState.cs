using System.Collections.Generic;
using UnityEngine;

namespace MyLesson19
{
    namespace StateMachineSystem.Locomotion
    {
        public class FallState : StateBase
        {
            private readonly CharacterController _characterController;

            private Vector3 _jumpDirection;
            private Vector3 _fallDirection;

            private bool _isGrounded;
            private float _horizontalSpeed;
            private Vector3 _velocity;
            private Vector3 _direction;
            private float _drag;

            private AudioSource _audioSource;
            private AudioClip _land;

            public FallState(
                StateMachine stateMachine,
                byte stateId,
                CharacterController characterController,
                float drag,
                AudioSource audioSource,
                AudioClip land) : base(stateMachine, stateId)
            {
                _characterController = characterController;
                _drag = drag;

                conditions = new List<IStateCondition> { new BaseCondition((byte)LocomotionState.Idle, Landed) };
                
                _audioSource = audioSource;
                _land = land;
            }

            public override void Enter()
            {
                _velocity = _characterController.velocity;
                _velocity.y = 0f;
                _direction = _velocity.normalized;
            }

            protected override void OnUpdate(float deltaTime)
            {
                float verticalVelocity = _velocity.y;
                _velocity.y = 0f;
                _velocity = Vector3.Lerp(_velocity, Vector3.zero, _drag * deltaTime);
                _velocity.y = verticalVelocity;

                _velocity += Physics.gravity * deltaTime;

                //Debug.Log($"[FallState] Velocity: {_velocity}");

                _ = _characterController.Move(_velocity * deltaTime);

                _audioSource.PlayOneShot(_land);
            }

            public override void Dispose()
            {
            }

            private bool Landed()
            {
                return _characterController.isGrounded;
            }
        }
    }
}