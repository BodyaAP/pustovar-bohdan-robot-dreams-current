using System;
using System.Collections;
using System.Collections.Generic;
using MyLesson19.BehaviourTreeSystem;
using MyLesson19.DefendFlag;
using MyLesson19.StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace MyLesson19
{
    public class GameModeService : MonoServiceBase, IModeService
    {
        public event Action<bool> OnComplete;

        public override Type Type { get; } = typeof(IModeService);

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void Start()
        {
            ServiceLocator.Instance.GetService<IPlayerService>().Player.Health.OnDeath += PlayerDeathHandler;
            enabled = false;
        }

        public void Begin()
        {
            enabled = true;
        }

        private void Update()
        {
            //TODO here there will be a check for the number of enemies in the scene, if there are zero the player will win (тут буде перевірка на кількість ворогів в сцені, якщо їх нуль гравець переміг)
        }

        private void PlayerDeathHandler()
        {
            enabled = false;
            ServiceLocator.Instance.GetService<StateMachineSystem.InputController>().DisableEscape();
            OnComplete?.Invoke(false);
        }
    }
}
