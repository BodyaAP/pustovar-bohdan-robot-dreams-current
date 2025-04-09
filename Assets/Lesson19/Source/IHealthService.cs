using MyLesson19.MainMenu;
using MyLesson19.StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace MyLesson19
{
    namespace BehaviourTreeSystem
    {
        public interface IHealthService : IService
        {
            IHealth this[Collider collider] { get; }

            void AddCharacter(IHealth character);

            void RemoveCharacter(IHealth character);

            bool GetHealth(Collider characterController, out Dummies.Health health);
        }
    }
}