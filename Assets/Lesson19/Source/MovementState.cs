using System.Collections.Generic;
using MyLesson19.StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace MyLesson19
{
    namespace StateMachineSystem.Locomotion
    {
        public class MovementState : StateBase
        {
            private readonly CharacterController _characterController;
            private readonly Transform _transform;
            private readonly float _speed;

            private readonly AudioSource _source;
            private readonly AudioClip _footstep;

            private bool _grounded;

            private Vector3 _localDirection;

            private InputController _inputController;

            public MovementState(
                StateMachine stateMachine,
                byte stateId,
                CharacterController characterController,
                Transform transform,
                float speed,
                AudioSource source,
                AudioClip footstep) : base(stateMachine, stateId)
            {
                _characterController = characterController;
                _transform = transform;
                _speed = speed;

                _source = source;
                _footstep = footstep;

                conditions = new List<IStateCondition>
                {
                    new BaseCondition((byte)LocomotionState.Idle, IsIdle),
                    new BaseCondition((byte)LocomotionState.Fall, IsFalling)
                };

                _inputController = ServiceLocator.Instance.GetService<InputController>();

                _inputController.OnMoveInput += MoveHandler;
            }

            private void MoveHandler(Vector2 input)
            {
                _localDirection = new Vector3(input.x, 0, input.y);
            }

            protected override void OnUpdate(float deltaTime)
            {
                Vector3 forward = _transform.forward;
                Vector3 right = _transform.right;

                Vector3 direction = forward * _localDirection.z + right * _localDirection.x;

                //_grounded = _characterController.SimpleMove(direction * _speed);
                _ = _characterController.Move((direction * _speed + Physics.gravity) * deltaTime);

                _source.PlayOneShot(_footstep);
            }

            public override void Dispose()
            {
                _inputController.OnMoveInput -= MoveHandler;
            }

            private bool IsIdle()
            {
                return Mathf.Approximately(_localDirection.sqrMagnitude, 0f);
            }

            private bool IsFalling()
            {
                return !_characterController.isGrounded;
            }
        }
    }
}