using System;

namespace MyLesson19
{
    namespace StateMachineSystem
    {
        public interface IState : IDisposable
        {
            byte StateId { get; }

            void Enter();
            void Update(float deltaTime);
            void Exit();
        }
    }
}