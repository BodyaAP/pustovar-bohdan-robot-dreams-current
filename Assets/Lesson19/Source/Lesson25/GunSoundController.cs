using System;
using MyLesson19.StateMachineSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MyLesson19
{
    namespace AudioSystem
    {
        /// <summary>
        /// Script that subscribes to events of HitScanGun in order to play sounds
        /// when gun shots and hits
        /// </summary>
        public class GunSoundController : MonoBehaviour
        {
            [SerializeField] private AudioSource _shootSource;
            [SerializeField] private AudioSource _hitSource;
            [SerializeField] private AudioClip[] _shootClips;
            [SerializeField] private AudioClip[] _hitClips;
            [SerializeField] private HitScanGunBase _gun;
            [SerializeField] private Transform _muzzle;

            private void Start()
            {
                _gun.OnShot += ShotHandler;
                _gun.OnHitPrecise += HitHandler;
            }

            private void ShotHandler()
            {
                _shootSource.transform.position = _muzzle.position;
                _shootSource.PlayOneShot(_shootClips[Random.Range(0, _shootClips.Length)]);
            }

            private void HitHandler(RaycastHit hit)
            {
                //Debug.Break();
                _hitSource.transform.position = hit.point;
                _hitSource.PlayOneShot(_hitClips[Random.Range(0, _hitClips.Length)]);
            }
        }
    }
}