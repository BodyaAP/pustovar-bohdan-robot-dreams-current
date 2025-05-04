using MyLesson19.StateMachineSystem.ServiceLocatorSystem;
using System.Collections;
using System.Collections.Generic;
using MyLesson19.AudioSystem;
using UnityEngine;
using UnityEngine.UI;

namespace MyLesson19
{
    public class Settings : MonoBehaviour
    {
        [SerializeField]
        public struct SoundSlider
        {
            public SoundType type;
            public Slider slider;
        }

        [SerializeField] private Canvas _canvas;
        [SerializeField] private SoundSlider[] _sliders;

        private StateMachineSystem.InputController _inputController;
        private ISoundService _soundService;

        public bool Enabled
        {
            get => _canvas.enabled;
            set
            {
                if (_canvas.enabled == value)
                    return;
                _canvas.enabled = value;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            _soundService = ServiceLocator.Instance.GetService<ISoundService>();

            for (int i = 0; i < _sliders.Length; ++i)
            {
                SoundType soundType = _sliders[i].type;
                Slider slider = _sliders[i].slider;
                slider.SetValueWithoutNotify(_soundService.GetVolume(soundType));
                slider.onValueChanged.AddListener((value) => SliderHandler(soundType, value));
            }

            Enabled = false;
        }

        private void SliderHandler(SoundType soundType, float value)
        {
            _soundService.SetVolume(soundType, value);
        }
    }
}
