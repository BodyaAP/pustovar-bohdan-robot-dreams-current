using System;
using MyLesson19.StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace MyLesson19
{
    namespace StateMachineSystem
    {
        public class PlayerActivator : MonoBehaviour
        {
            [SerializeField] private GameObject _player;

            private void Start()
            {
                _player.SetActive(true);
                ServiceLocator.Instance.GetService<InputController>().enabled = true;
            }
        }
    }
}