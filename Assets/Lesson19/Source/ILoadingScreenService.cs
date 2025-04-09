using MyLesson19.StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MyLesson19
{
    namespace LoadingScreen
    {
        public interface ILoadingScreenService : IService
        {
            void BeginLoading(AsyncOperation asyncOperation);
            void BeginLoading(AsyncOperationHandle asyncOperation);
        }
    }
}