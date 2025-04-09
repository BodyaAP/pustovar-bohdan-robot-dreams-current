using System.Collections;
using System.Collections.Generic;
using MyLesson14;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyLesson16
{
    public class Dummy : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private GameObject _rootObject;
        [SerializeField] private float _healthBarDelayTime;

        [SerializeField] private ParticleSystem _destroyedEffect;

        [SerializeField] private HealthIndicator _healthIndicator;

        public Health Health => _health;
        public HealthIndicator HealthIndicator => _healthIndicator;

        // Start is called before the first frame update
        void Start()
        {
            _health.OnDeath += DeathHandler;

            PoolSystem.Create();

            if (_destroyedEffect != null)
            {
                PoolSystem.Instance.InitPool(_destroyedEffect, 16);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void DeathHandler()
        {
            StartCoroutine(DelayedDestroy());
        }

        private IEnumerator DelayedDestroy()
        {
            float time = 0f;
            float reciprocal = 1f / _healthBarDelayTime;

            while (time < _healthBarDelayTime)
            {
                yield return null;
                time += Time.deltaTime;
            }

            if (_destroyedEffect != null)
            {
                var effect = PoolSystem.Instance.GetInstance<ParticleSystem>(_destroyedEffect);
                effect.time = 0.0f;
                effect.Play();
                effect.transform.position = transform.position;
            }

            Destroy(_rootObject);
        }
    }
}