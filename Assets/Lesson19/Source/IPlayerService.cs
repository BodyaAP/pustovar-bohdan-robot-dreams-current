using MyLesson19.StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace MyLesson19
{
    namespace BehaviourTreeSystem
    {
        public interface IPlayerService : IService
        {
            PlayerController Player { get; }
            bool IsPlayer(Collider collider);
        }
    }
}