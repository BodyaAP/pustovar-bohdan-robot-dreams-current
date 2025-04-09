using System;
using MyLesson19.StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace MyLesson19
{
    namespace BehaviourTreeSystem
    {
        public class CameraService : MonoServiceBase, ICameraService
        {
            [SerializeField] private Camera _camera;
            public override Type Type { get; } = typeof(ICameraService);
            public Camera Camera => _camera;
        }
    }
}