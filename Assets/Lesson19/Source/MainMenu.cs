using MyLesson19.LoadingScreen;
using MyLesson19.StateMachineSystem.GameStateSystem;
using MyLesson19.StateMachineSystem.SceneManagement;
using MyLesson19.StateMachineSystem.ServiceLocatorSystem;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MyLesson19
{
    namespace StateMachineSystem.UserInterface
    {
        public class MainMenu : MonoBehaviour
        {
            [SerializeField] private Button _trainingRangeButton;
            [SerializeField] private Button _quitButton;

            private void Awake()
            {
                _trainingRangeButton.onClick.AddListener(TrainingRangeButtonHandler);
                _quitButton.onClick.AddListener(QuitButtonHandler);
            }

            private void TrainingRangeButtonHandler()
            {
                ServiceLocator.Instance.GetService<ISceneManager>().onSceneLoad += SceneLoadHandler;
                ServiceLocator.Instance.GetService<IGameStateProvider>().SetGameState(GameState.Gameplay);
            }

            private void QuitButtonHandler()
            {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            }

            private void SceneLoadHandler(AsyncOperation asyncOperation)
            {
                ServiceLocator.Instance.GetService<ISceneManager>().onSceneLoad -= SceneLoadHandler;
                ServiceLocator.Instance.GetService<ILoadingScreenService>()?.BeginLoading(asyncOperation);
            }
        }
    }
}