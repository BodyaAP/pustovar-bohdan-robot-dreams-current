using System;
using System.Numerics;
using MyLesson19.PhysX;
using PhysX;
using MyLesson19.StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace MyLesson19
{
    namespace BehaviourTreeSystem
    {
        public class Playerdar : MonoBehaviour
        {
            public enum State
            {
                Scanning,
                Chasing,
                Searching,
            }

            [SerializeField] private EnemyController _enemyController;
            [SerializeField] private float _range;
            [SerializeField] private float _angle;

            private float _cosine;

            private Transform _transform;
            private IPlayerService _playerService;
            private IBasePlayerService _basePlayerService;

            private TargetableBase _currentTarget;
            private Vector3 _stormTarget;
            private bool _hasTarget;
            private bool _seesTarget;
            private Vector3 _lastTargetPosition;

            private float _distanceTarget = 1000f;

            private State _currentState;

            public State CurrentState
            {
                get => _currentState;
                set
                {
                    if (_currentState == value)
                        return;
                    _currentState = value;
                    _enemyController.ComputeBehaviour();
                }
            }

            public TargetableBase CurrentTarget => _currentTarget;
            public Vector3 StormTarget => _stormTarget;
            public bool HasTarget => _hasTarget;
            public bool SeesTarget => _seesTarget;
            public Vector3 LastTargetPosition => _lastTargetPosition;

            public float DistanceTarget => _distanceTarget;

            //private void Start()
            private void OnEnable()
            {
                _transform = transform;
                _playerService = ServiceLocator.Instance.GetService<IPlayerService>();
                _cosine = Mathf.Cos(_angle * Mathf.Deg2Rad);

                _enemyController.Health.OnHealthChanged += HealthChangedHandler;
                //_stormTarget = new Vector3(15, 0, 50); //Bad solution
                _basePlayerService = ServiceLocator.Instance.GetService<IBasePlayerService>();
                _stormTarget = _basePlayerService.Base.Targetable.TargetPivot.position;
            }

            private void OnDisable()
            {
                _enemyController.Health.OnHealthChanged -= HealthChangedHandler;
                _hasTarget = false;
                _seesTarget = false;
            }

            private void FixedUpdate()
            {
                switch (CurrentState)
                {
                    case State.Scanning:
                        ScanningUpdate();
                        break;
                    case State.Chasing:
                        ChasingUpdate();
                        break;
                    case State.Searching:
                        SearchingUpdate();
                        break;
                }

                //Debug.Log(CurrentState);
                //Debug.Log(CurrentTarget);
                //Debug.Log(HasTarget);
                //Debug.Log(SeesTarget);

                //Debug.Log(_playerService.Player.Targetable != null ? true : false);
                //Debug.Log(DistanceTarget);
            }

            private void ScanningUpdate()
            {
                if (!CheckTarget(_playerService.Player.Targetable))
                    return;

                _currentTarget = _playerService.Player.Targetable;
                _hasTarget = true;
                _seesTarget = true;
                _distanceTarget = GetDistanceTarget(_currentTarget);
                CurrentState = State.Chasing;
            }

            private void ChasingUpdate()
            {
                _lastTargetPosition = _currentTarget.TargetPivot.position;

                if (CheckTarget(_currentTarget))
                {
                    _distanceTarget = GetDistanceTarget(_currentTarget);
                    return;
                }

                _seesTarget = false;
                CurrentState = State.Searching;
            }

            private void SearchingUpdate()
            {
                if (!CheckTarget(_currentTarget))
                    return;

                _seesTarget = true;
                _distanceTarget = GetDistanceTarget(_currentTarget);
                CurrentState = State.Chasing;
            }

            public void LookAround()
            {
                if (_hasTarget)
                {
                    if (CheckTarget(_currentTarget, false))
                    {
                        _seesTarget = true;
                        _distanceTarget = GetDistanceTarget(_currentTarget);
                        CurrentState = State.Chasing;
                    }
                    else
                    {
                        _seesTarget = false;
                        CurrentState = State.Scanning;
                    }
                }
                else
                {
                    if (CheckTarget(_playerService.Player.Targetable, false))
                    {
                        _hasTarget = true;
                        _seesTarget = true;
                        _currentTarget = _playerService.Player.Targetable;
                        _distanceTarget = GetDistanceTarget(_currentTarget);
                        CurrentState = State.Chasing;
                    }
                    else
                    {
                        _seesTarget = false;
                        CurrentState = State.Scanning;
                    }
                }
            }

            private bool CheckTarget(TargetableBase targetable, bool useFov = true)
            {
                Vector3 position = _transform.position;
                Vector3 playerPosition = targetable.TargetPivot.position;

                Vector3 playerDirection = Vector3.ProjectOnPlane(playerPosition - position, Vector3.up);

                if (playerDirection.sqrMagnitude > _range * _range)
                    return false;

                playerDirection.Normalize();
                Vector3 forward = Vector3.ProjectOnPlane(_transform.forward, Vector3.up).normalized;

                if (useFov)
                {
                    if (Vector3.Dot(playerDirection, forward) < _cosine)
                        return false;
                }

                if (!Physics.Raycast(position, (playerPosition - position).normalized, out RaycastHit hit, _range))
                    return false;
                if (hit.collider != _playerService.Player.CharacterController)
                    return false;
                return true;
            }

            private float GetDistanceTarget(TargetableBase targetable)
            {
                //Vector3 position = _transform.position;
                //Vector3 playerPosition = targetable.TargetPivot.position;

                //return targetable != null ? Vector3.Distance(position, playerPosition) : 1000;

                if (targetable == null)
                {
                    return 1000;
                }

                Vector3 position = _transform.position;
                Vector3 playerPosition = targetable.TargetPivot.position;

                return Vector3.Distance(position, playerPosition);
            }

            private void HealthChangedHandler(int health)
            {
                LookAround();
            }
        }
    }
}