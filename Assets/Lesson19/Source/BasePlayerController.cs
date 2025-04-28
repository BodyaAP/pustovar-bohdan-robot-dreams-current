using MyLesson19.MainMenu;
using MyLesson19.PhysX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyLesson19
{
    public class BasePlayerController : MonoBehaviour
    {
        [SerializeField] private CharacterController _baseCharacterController;
        [SerializeField] private TargetableBase _targetable;
        [SerializeField] private Health _health;

        public CharacterController BaseCharacterController => _baseCharacterController;
        public TargetableBase Targetable => _targetable;
        public Health Health => _health;
    }
}
