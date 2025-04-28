using System;
using MyLesson19.StateMachineSystem.ServiceLocatorSystem;

namespace MyLesson19
{
    namespace DefendFlag
    {
        public interface IModeService : IService
        {
            event Action<bool> OnComplete;

            void Begin();
        }
    }
}