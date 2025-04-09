using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MyLesson14
{
    public class HealthSystem : MonoBehaviour
    {
        public event Action<Health> OnCharacterDeath;

        [SerializeField] private Health[] _healths;
        protected Dictionary<Collider, Health> _charactersHealth = new();

        public IEnumerable<Health> CharactersHealths => _charactersHealth.Values;

        protected virtual void Awake()
        {
            for (int i = 0; i < _healths.Length; i++)
            {
                Health health = _healths[i];
                _charactersHealth.Add(health.CharacterController, health);
                health.OnDeath += () => CharacterDeathHandler(health);
            }
        }

        public virtual bool GetHealth(Collider characterController, out Health health) =>
            _charactersHealth.TryGetValue(characterController, out health);

        protected void CharacterDeathHandler(Health health)
        {
            OnCharacterDeath?.Invoke(health);
        }

        /// <summary>
        /// Editor only method
        /// </summary>
        [ContextMenu("Find Healths")]
        private void FindHealths()
        {
#if UNITY_EDITOR
            _healths = FindObjectsOfType<Health>();
            EditorUtility.SetDirty(this);
#endif
        }

        public void AddCharacter(Health character)
        {
            _charactersHealth.Add(character.CharacterController, character);
            character.OnDeath += () => CharacterDeathHandler(character);
        }

        public void RemoveCharacter(Health character)
        {
            _charactersHealth.Remove(character.CharacterController);
        }
    }
}