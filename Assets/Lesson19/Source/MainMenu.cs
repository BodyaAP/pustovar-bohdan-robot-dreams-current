using MyLesson19.LoadingScreen;
using MyLesson19.StateMachineSystem.GameStateSystem;
using MyLesson19.StateMachineSystem.SceneManagement;
using MyLesson19.StateMachineSystem.ServiceLocatorSystem;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using UnityEngine.Rendering;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MyLesson19
{
    namespace StateMachineSystem.UserInterface
    {
        public class MainMenu : MonoBehaviour
        {
            [Serializable]
            public struct MenuAnimationData
            {
                public Vector2 closedPosition;
                public Vector2 openPosition;
            }

            [SerializeField] private Button _trainingRangeButton;
            [SerializeField] private Button _quitButton;

            [SerializeField] private Canvas _canvas;
            [SerializeField] private RectTransform _menuTransform;
            [SerializeField] private Button _settingsButton;
            [SerializeField] private Button _closeSettingsButton;
            [SerializeField] private Settings _soundMenu;
            [SerializeField] private RectTransform _settingsTransform;
            [SerializeField] private float _fadeDuration;
            [SerializeField] private MenuAnimationData _menuAnimation;
            [SerializeField] private MenuAnimationData _settingsAnimation;

            private Coroutine _fadeRoutine;

            private void Awake()
            {
                _trainingRangeButton.onClick.AddListener(TrainingRangeButtonHandler);
                _quitButton.onClick.AddListener(QuitButtonHandler);

                _settingsButton.onClick.AddListener(SettingsButtonHandler);
                _closeSettingsButton.onClick.AddListener(CloseSettingsButtonHandler);

                _canvas.enabled = true;
                _soundMenu.Enabled = false;
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
                ServiceLocator.Instance.GetService<ISceneManager>().onSceneLoad += SceneLoadHandler;
                ServiceLocator.Instance.GetService<ILoadingScreenService>()?.BeginLoading(asyncOperation);
            }

            private void SettingsButtonHandler()
            {
                if (_fadeRoutine != null)
                    StopCoroutine(_fadeRoutine);
                _fadeRoutine = StartCoroutine(OpenSettings());
            }

            private void CloseSettingsButtonHandler()
            {
                if (_fadeRoutine != null)
                    StopCoroutine(_fadeRoutine);
                _fadeRoutine = StartCoroutine(CloseSettings());
            }

            private IEnumerator OpenSettings()
            {
                _soundMenu.Enabled = true;
                _canvas.enabled = true;
                EvaluateMenu(1f);
                EvaluateSettings(0f);

                float time = 0f;
                float reciprocal = 1f / _fadeDuration;

                while (time < _fadeDuration)
                {
                    float progress = time * reciprocal;
                    EvaluateMenu(1f - progress);
                    EvaluateSettings(progress);
                    yield return null;
                    time += Time.deltaTime;
                }

                EvaluateMenu(0f);
                EvaluateSettings(1f);

                _canvas.enabled = false;
                _fadeRoutine = null;
            }

            private IEnumerator CloseSettings()
            {
                _soundMenu.Enabled = true;
                _canvas.enabled = true;
                EvaluateMenu(0f);
                EvaluateSettings(1f);

                float time = 0f;
                float reciprocal = 1f / _fadeDuration;

                while (time < _fadeDuration)
                {
                    float progress = time * reciprocal;
                    EvaluateMenu(progress);
                    EvaluateSettings(1f - progress);
                    yield return null;
                    time += Time.deltaTime;
                }

                EvaluateMenu(1f);
                EvaluateSettings(0f);

                _soundMenu.Enabled = false;
                _fadeRoutine = null;
            }

            private void EvaluateMenu(float progress)
            {
                Vector2 menuPosition = Vector2.Lerp(_menuAnimation.closedPosition, _menuAnimation.openPosition, progress);
                _menuTransform.anchoredPosition = menuPosition;
            }

            private void EvaluateSettings(float progress)
            {
                Vector2 settingsPosition = Vector2.Lerp(_settingsAnimation.closedPosition, _settingsAnimation.openPosition, progress);
                _settingsTransform.anchoredPosition = settingsPosition;
            }
        }
    }
}