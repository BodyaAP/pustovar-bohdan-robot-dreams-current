using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayMenu : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Button _confrimButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private string _lobbySceneName;
    [SerializeField] private InputController _inputController;

    public bool Enabled
    {
        get => _canvas.enabled;
        set
        {
            if (_canvas.enabled == value)
            {
                return;
            }

            _canvas.enabled = value;
            _inputController.enabled = !value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InputController.OnEscape += EscapeHandler;
    }

    private void Awake()
    {
        _confrimButton.onClick.AddListener(ConfirmButtonHandler);
        _cancelButton.onClick.AddListener(CancelButtonHandler);
    }

    private void EscapeHandler()
    {
        Enabled = !Enabled;
    }

    private void ConfirmButtonHandler()
    {
        SceneManager.LoadSceneAsync(_lobbySceneName, LoadSceneMode.Single);
    }

    private void CancelButtonHandler()
    {
        Enabled = false;
    }
}
