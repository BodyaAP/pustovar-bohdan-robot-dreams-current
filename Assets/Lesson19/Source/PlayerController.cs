using MyLesson19.MainMenu;
using MyLesson19.PhysX;
using PhysX;
using UnityEngine;

namespace MyLesson19
{
    namespace BehaviourTreeSystem
    {
        public class PlayerController : MonoBehaviour
        {
            [SerializeField] private CharacterController _characterController;
            [SerializeField] private TargetableBase _targetable;
            [SerializeField] private Health _health;

            public CharacterController CharacterController => _characterController;
            public TargetableBase Targetable => _targetable;
            public Health Health => _health;
        }
    }
}