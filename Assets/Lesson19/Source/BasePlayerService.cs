using System;
using System.Collections;
using System.Collections.Generic;
using MyLesson19.StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace MyLesson19
{
    public class BasePlayerService : MonoServiceBase, IBasePlayerService
    {
        [SerializeField] private BasePlayerController _basePlayerController;

        public override Type Type { get; } = typeof(IBasePlayerService);

        public BasePlayerController Base => _basePlayerController;

        public Dictionary<Collider, BasePlayerController> _baseControllers = new();

        protected override void Awake()
        {
            base.Awake();
            _baseControllers.Add(_basePlayerController.BaseCharacterController, _basePlayerController);
        }

        public bool IsBase(Collider collider)
        {
            return _baseControllers.ContainsKey(collider);
        }
    }
}
