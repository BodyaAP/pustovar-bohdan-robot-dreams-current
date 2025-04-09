using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MyLesson16
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private string _sceneName;

        // Start is called before the first frame update
        void Awake()
        {
            _newGameButton.onClick.AddListener(NewGameButtonHandler);
            _exitButton.onClick.AddListener(ExitButtonHandler);
        }

        private void NewGameButtonHandler()
        {
            SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Single);
        }

        private void ExitButtonHandler()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}