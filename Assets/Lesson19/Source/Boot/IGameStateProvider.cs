using System;
using MyLesson19.StateMachineSystem.ServiceLocatorSystem;

namespace MyLesson19
{
    namespace StateMachineSystem.GameStateSystem
    {
        public interface IGameStateProvider : IService
        {
            event Action<GameState> OnGameStateChanged;

            GameState GameState { get; }

            void SetGameState(GameState gameState);
        }
    }
}