using System;
using UnityEngine;

namespace MyLesson19
{
    namespace StateMachineSystem.Locomotion
    {
        public class LocomotionController : MonoBehaviour
        {
            public event Action<LocomotionState> OnStateChanged;

            [SerializeField] private CharacterController _characterController;
            [SerializeField] private float _speed;
            [SerializeField] private float _drag;
            [SerializeField] private Vector2 _jumpSpeed;

            [SerializeField] private AudioSource _audioSource;
            [SerializeField] private AudioClip _footstep;
            [SerializeField] private AudioClip _jump;
            [SerializeField] private AudioClip _land;

            private StateMachine _stateMachine;

            public string CurrentState => _stateMachine == null ? "[NULL]" : _stateMachine.CurrentState.GetType().Name;

            public StateMachine StateMachine => _stateMachine;

            private void Start()
            {
                _stateMachine = new StateMachine();
                _stateMachine.AddState((byte)LocomotionState.Idle,
                    new IdleState(_stateMachine, (byte)LocomotionState.Idle, _characterController));

                _stateMachine.AddState((byte)LocomotionState.Movement,
                    new MovementState(_stateMachine, (byte)LocomotionState.Movement, _characterController,
                        _characterController.transform, _speed, _audioSource, _footstep));

                _stateMachine.AddState((byte)LocomotionState.Fall,
                    new FallState(_stateMachine, (byte)LocomotionState.Fall, _characterController, _drag, _audioSource, _land));

                _stateMachine.AddState((byte)LocomotionState.Jump,
                    new JumpState(_stateMachine, (byte)LocomotionState.Jump, _characterController,
                        _drag, _jumpSpeed.y, _jumpSpeed.x, _audioSource, _jump));

                _stateMachine.InitState((byte)LocomotionState.Idle);

                _stateMachine.OnStateChange += StateChangeHandler;
            }

            private void FixedUpdate()
            {
                _stateMachine.Update(Time.fixedDeltaTime);
            }

            private void OnDestroy()
            {
                _stateMachine?.Dispose();
            }

            private void StateChangeHandler(byte stateId)
            {
                OnStateChanged?.Invoke((LocomotionState)stateId);
            }
        }
    }
}