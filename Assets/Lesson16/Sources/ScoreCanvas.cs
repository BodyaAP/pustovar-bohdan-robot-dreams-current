using System.Collections;
using System.Collections.Generic;
using MyLesson12;
using MyLesson14;
using TMPro;
using UnityEngine;

namespace MyLesson16
{
    public class ScoreCanvas : MonoBehaviour
    {
        [SerializeField] private ScoreSystem _scoreSystem;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _kills;
        [SerializeField] private TextMeshProUGUI _death;
        [SerializeField] private TextMeshProUGUI _assists;
        [SerializeField] private TextMeshProUGUI _accuracy;

        private bool _updateRequested;

        // Start is called before the first frame update
        void Start()
        {
            _scoreSystem.OnDataUpdated += DataUpdateHandler;
            InputController.OnScoreInput += ScoreInputHandler;
            _updateRequested = true;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (!_updateRequested)
            {
                return;
            }

            _updateRequested = false;

            _kills.text = _scoreSystem.KDA.x.ToString();
            _death.text = _scoreSystem.KDA.y.ToString();
            _assists.text = _scoreSystem.KDA.z.ToString();
            _accuracy.text = _scoreSystem.Accuracy.ToString();
        }

        private void DataUpdateHandler()
        {
            _updateRequested = true;
        }

        private void ScoreInputHandler(bool show)
        {
            _canvasGroup.alpha = show ? 1f : 0f;
        }
    }
}