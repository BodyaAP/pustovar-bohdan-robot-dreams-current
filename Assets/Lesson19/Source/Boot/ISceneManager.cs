using System;
using MyLesson19.StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace MyLesson19
{
    namespace StateMachineSystem.SceneManagement
    {
        public interface ISceneManager : IService
        {
            event Action<AsyncOperation> onSceneLoad;

            void SetScene(Scenes scene);
            void OnSceneLoad(AsyncOperation operation);
        }
    }
}