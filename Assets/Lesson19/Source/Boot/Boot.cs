using System;
using System.Collections;
using MyLesson19.StateMachineSystem.GameStateSystem;
using MyLesson19.StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace MyLesson19
{
    namespace StateMachineSystem
    {
        public class Boot : MonoBehaviour
        {
            public IEnumerator Start()
            {
                yield return null;
                ServiceLocator.Instance.GetService<IGameStateProvider>().SetGameState(GameState.MainMenu);
            }
        }
    }
}