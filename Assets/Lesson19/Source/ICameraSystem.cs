using MyLesson19.StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace MyLesson19
{
    namespace BehaviourTreeSystem
    {
        public interface ICameraService : IService
        {
            Camera Camera { get; }
        }
    }
}