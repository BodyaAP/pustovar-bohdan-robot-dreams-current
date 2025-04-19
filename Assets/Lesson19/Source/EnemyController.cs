using BehaviourTreeSystem.BehaviourStates;
using MyLesson19.BehaviourTreeSystem.BehaviourStates;
using MyLesson19.Dummies;
using MyLesson19.MainMenu;
using MyLesson19.Dummies;
using MyLesson19.MainMenu;
using MyLesson19.StateMachineSystem;
using MyLesson19.StateMachineSystem;
using MyLesson19.StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;
using UnityEngine.AI;
using Health = MyLesson19.MainMenu.Health;
using MyLesson16;
using System.Collections;
using MyLesson19.DefendFlag;

namespace MyLesson19
{
    namespace BehaviourTreeSystem
    {
        public class EnemyController : MonoBehaviour
        {
            [SerializeField] private HealthIndicator _healthIndicator;
            [SerializeField] private EnemyData _data;
            [SerializeField] private NavMeshAgent _navMeshAgent;
            [SerializeField] private CharacterController _characterController;
            [SerializeField] private Transform _characterTransform;
            [SerializeField] private Health _health;
            [SerializeField] private GameObject _rootObject;
            [SerializeField] private Transform _meshRendererTransform;
            [SerializeField] private Transform _fallMark;
            [SerializeField] private Transform _weaponTransform;
            [SerializeField] private HitScanGun _hitScanGun;
            [SerializeField] private WeaponData _weaponData;

            [SerializeField] private Playerdar _playerdar;

            [SerializeField, ReadOnly] private float _patrolStamina;
            [SerializeField, ReadOnly] private EnemyBehaviour _currentBehaviour;

            [SerializeField] private ParticleSystem _destroyedEffect;
            [SerializeField] private bool _isStormtrooper = false;

            private INavPointProvider _navPointProvider;
            //private ObjectPool _objectPool;
            //private System.Action _onDeathAction;

            protected BehaviourTree _behaviourTree;
            protected StateMachine _behaviourMachine;

            public StateMachine BehaviourMachine => _behaviourMachine;

            public float PatrolStamina
            {
                get => _patrolStamina;
                set { _patrolStamina = Mathf.Clamp(value, 0, _data.MaxPatrolStamina); }
            }

            public EnemyData Data => _data;
            public NavMeshAgent NavMeshAgent => _navMeshAgent;
            public INavPointProvider NavPointProvider => _navPointProvider;
            public CharacterController CharacterController => _characterController;
            public Transform CharacterTransform => _characterTransform;
            public IHealth Health => _health;
            public Playerdar Playerdar => _playerdar;
            public Transform FallMark => _fallMark;
            public GameObject RootObject => _rootObject;
            public Transform MeshRendererTransform => _meshRendererTransform;
            public Transform WeaponTransform => _weaponTransform;
            public HitScanGun HitScanGun => _hitScanGun;
            public WeaponData WeaponData => _weaponData;

            public bool IsStormtrooper => _isStormtrooper;

            private void Awake()
            {
                _navMeshAgent.updatePosition = false;
                _navMeshAgent.updateRotation = false;
                _navMeshAgent.avoidancePriority = Random.Range(0, 100);

                InitStateMachine();
                _behaviourMachine.OnStateChange += StateChangeHandler;

                InitBehaviourTree();

                _health.OnDeath += HealthDeathHandler;

                PoolSystem.Create();

                if (_destroyedEffect != null)
                {
                    PoolSystem.Instance.InitPool(_destroyedEffect, 16);
                }
            }

            protected virtual void InitStateMachine()
            {
                _behaviourMachine = new StateMachine();

                _behaviourMachine.AddState((byte)EnemyBehaviour.Deciding,
                    new DecisionBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Deciding, this));
                _behaviourMachine.AddState((byte)EnemyBehaviour.Idle,
                    new IdleBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Idle, this));
                _behaviourMachine.AddState((byte)EnemyBehaviour.Patrol,
                    new PatrolBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Patrol, this));
                _behaviourMachine.AddState((byte)EnemyBehaviour.Search,
                    new SearchBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Search, this));

                /*_behaviourMachine.AddState((byte)EnemyBehaviour.Attack,
                    new AttackBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Attack, this));*/
                _behaviourMachine.AddState((byte)EnemyBehaviour.Attack,
                    new ShootBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Attack, this));

                _behaviourMachine.AddState((byte)EnemyBehaviour.Death,
                    new DeathBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Death, this));

                _behaviourMachine.AddState((byte)EnemyBehaviour.Melee,
                    new MeleeBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Melee, this));

                _behaviourMachine.AddState((byte)EnemyBehaviour.Storm,
                    new StormBehavior(_behaviourMachine, (byte)EnemyBehaviour.Storm, this));
            }

            protected virtual void InitBehaviourTree()
            {

                BehaviourLeaf patrolLeaf = new BehaviourLeaf((byte)EnemyBehaviour.Patrol);
                BehaviourLeaf stormLeaf = new BehaviourLeaf((byte)EnemyBehaviour.Storm);

                BehaviourBranch defenderBranch = new BehaviourBranch(stormLeaf, patrolLeaf, DefenderCondition);

                BehaviourLeaf idleLeaf = new BehaviourLeaf((byte)EnemyBehaviour.Idle);

                BehaviourBranch patrolBranch = new BehaviourBranch(defenderBranch, idleLeaf, PatrolStaminaCondition);

                BehaviourLeaf attackLeaf = new BehaviourLeaf((byte)EnemyBehaviour.Attack);
                BehaviourLeaf meleeLeaf = new BehaviourLeaf((byte)EnemyBehaviour.Melee);

                BehaviourBranch attackBranch = new BehaviourBranch(attackLeaf, meleeLeaf, AttackTargetCondition);

                BehaviourLeaf searchLeaf = new BehaviourLeaf((byte)EnemyBehaviour.Search);

                BehaviourBranch seesTarget = new BehaviourBranch(attackBranch, searchLeaf, SeesTargetCondition);

                BehaviourBranch hasTarget = new BehaviourBranch(seesTarget, patrolBranch, HasTargetCondition);

                _behaviourTree = new BehaviourTree(hasTarget);

                ComputeBehaviour();
            }

            private void FixedUpdate()
            {
                _behaviourMachine.Update(Time.fixedDeltaTime);
                //Debug.Log(_currentBehaviour);
            }

            private void StateChangeHandler(byte stateId)
            {
                _currentBehaviour = (EnemyBehaviour)stateId;

                //Debug.Log($"Enemy state changed: {_currentBehaviour}");
            }

            public void Initialize(INavPointProvider navPointProvider, Camera camera)
            {
                _navPointProvider = navPointProvider;
                _healthIndicator.Billboard.SetCamera(camera);
            }

            public void ComputeBehaviour()
            {
                if (_behaviourTree == null)
                    return;

                byte behaviourTreeId = _behaviourTree.GetBehaviourId();
                EnemyBehaviour treeBehaviour = (EnemyBehaviour)behaviourTreeId;

                //Debug.Log($"Compute behaviour: {treeBehaviour}");

                //_behaviourMachine.SetState(_behaviourTree.GetBehaviourId());
                _behaviourMachine.SetState(behaviourTreeId);
            }

            public void RestorePatrolStamina()
            {
                _patrolStamina = _data.MaxPatrolStamina;
            }

            protected bool PatrolStaminaCondition()
            {
                return _patrolStamina > 0;
            }

            protected bool HasTargetCondition()
            {
                return _playerdar.HasTarget;
            }

            protected bool SeesTargetCondition()
            {
                return _playerdar.SeesTarget;
            }

            protected bool AttackTargetCondition()
            {
                return _playerdar.DistanceTarget >= Data.MeleeBehaviourRange;
            }

            protected bool DefenderCondition()
            {
                return _isStormtrooper;
            }

            protected void HealthDeathHandler()
            {
                _behaviourTree = null;
                _behaviourMachine.ForceState((byte)EnemyBehaviour.Death);
                ServiceLocator.Instance.GetService<IHealthService>().RemoveCharacter(_health);



                StartCoroutine(DelayedDestroy());
            }

            private IEnumerator DelayedDestroy()
            {
                float time = 0f;
                float reciprocal = 1f / _data.HealthBarDelayTime;

                while (time < _data.HealthBarDelayTime)
                {
                    yield return null;
                    time += Time.deltaTime;
                }

                if (_destroyedEffect != null)
                {
                    var effect = PoolSystem.Instance.GetInstance<ParticleSystem>(_destroyedEffect);
                    effect.time = 0.0f;
                    effect.Play();
                    effect.transform.position = _characterTransform.position;
                }

                Destroy(_rootObject);
                //_objectPool.ReturnToPool(this);
            }

            //public void SetPool(ObjectPool pool)
            //{
            //    _objectPool = pool;
            //}

            //public void SetOnDeathHandler(System.Action onDeath)
            //{
            //    if (_onDeathAction != null)
            //        _health.OnDeath -= _onDeathAction;

            //    _onDeathAction = onDeath;
            //    _health.OnDeath += _onDeathAction;
            //}
        }
    }
}