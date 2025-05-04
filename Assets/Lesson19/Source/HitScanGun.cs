using System;
using System.Collections;
using System.Numerics;
using Shooting;
//using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Vector4 = UnityEngine.Vector4;

namespace MyLesson19
{
    namespace BehaviourTreeSystem
    {
        public class HitScanGun : MonoBehaviour
        {
            public event Action<Collider> OnHit;
            public event Action OnShot;

            public event Action<Collider> OnMelee;

            [SerializeField] protected HitscanShotAspect _shotPrefab;
            [SerializeField] protected Transform _muzzleTransform;
            [SerializeField] protected float _decaySpeed;
            [SerializeField] protected Vector3 _shotScale;
            [SerializeField] protected float _shotRadius;
            [SerializeField] protected float _shotVisualDiameter;
            [SerializeField] protected string _tilingName;
            [SerializeField] protected float _range;
            [SerializeField] protected LayerMask _layerMask;

            [SerializeField] protected float _meleeDistance;

            [SerializeField] private GameObject _muzzleFlashPrefab;
            [SerializeField] private bool _unparentMuzzleFlash;
            [SerializeField] private GameObject _impactVfx;
            [SerializeField] private float _impactVfxLifetime = 5f;
            [SerializeField] private float _impactVfsSpawnOffset = 0.1f;

            [SerializeField] private AudioSource _audioSource;
            [SerializeField] private AudioClip _audioShoot;


            protected int _tilingId;

            protected virtual void Start()
            {
                _tilingId = Shader.PropertyToID(_tilingName);
            }

            public virtual void Shoot()
            {
                Vector3 muzzlePosition = _muzzleTransform.position;
                Vector3 muzzleForward = _muzzleTransform.forward;
                Ray ray = new Ray(muzzlePosition, muzzleForward);
                Vector3 hitPoint = muzzlePosition + muzzleForward * _range;

                if (_muzzleFlashPrefab != null)
                {
                    GameObject muzzleFlashInstance =
                        Instantiate(_muzzleFlashPrefab, muzzlePosition, _muzzleTransform.rotation, _muzzleTransform);

                    if (_unparentMuzzleFlash)
                    {
                        muzzleFlashInstance.transform.SetParent(null);
                    }

                    Destroy(muzzleFlashInstance, 2f);
                }

                if (Physics.SphereCast(ray, _shotRadius, out RaycastHit hitInfo, _range, _layerMask))
                {
                    Vector3 directVector = hitInfo.point - _muzzleTransform.position;
                    Vector3 rayVector = Vector3.Project(directVector, ray.direction);
                    hitPoint = muzzlePosition + rayVector;

                    if (_impactVfx)
                    {
                        GameObject impactVfxInstance = Instantiate(_impactVfx,
                            hitInfo.point + (hitInfo.normal * _impactVfsSpawnOffset),
                            UnityEngine.Quaternion.LookRotation(hitInfo.normal));
                        if (_impactVfxLifetime > 0)
                        {
                            Destroy(impactVfxInstance.gameObject, _impactVfxLifetime);
                        }
                    }

                    OnHit?.Invoke(hitInfo.collider);
                }

                HitscanShotAspect shot = Instantiate(_shotPrefab, hitPoint, _muzzleTransform.rotation);
                shot.distance = (hitPoint - _muzzleTransform.position).magnitude;
                shot.outerPropertyBlock = new MaterialPropertyBlock();
                StartCoroutine(ShotRoutine(shot));

                _audioSource.PlayOneShot(_audioShoot);

                OnShot?.Invoke();
            }

            protected IEnumerator ShotRoutine(HitscanShotAspect shot)
            {
                float interval = _decaySpeed * Time.deltaTime;
                while (shot.distance >= interval)
                {
                    EvaluateShot(shot);
                    yield return null;
                    shot.distance -= interval;
                    interval = _decaySpeed * Time.deltaTime;
                }

                Destroy(shot.gameObject);
            }

            protected void EvaluateShot(HitscanShotAspect shot)
            {
                shot.Transform.localScale = new Vector3(_shotScale.x, _shotScale.y, shot.distance * 0.5f);
                Vector4 tiling = Vector4.one;
                tiling.y = shot.distance * 0.5f / _shotVisualDiameter;
                shot.outerPropertyBlock.SetVector(_tilingId, tiling);
                shot.Outer.SetPropertyBlock(shot.outerPropertyBlock);
            }

            /// <summary>
            /// Цей код потрібно винести в окремий файл, щоб його можна було
            /// використовувати для зброї ближнього бою
            /// </summary>
            public void Melee()
            {
                Vector3 targetDirection = Vector3.zero;
                Ray ray = new Ray(_muzzleTransform.position, _muzzleTransform.forward);
                if (Physics.SphereCast(ray, 0.5f, out RaycastHit hitInfo, _meleeDistance))
                {
                    targetDirection = (hitInfo.point  - _muzzleTransform.position).normalized;
                    OnMelee?.Invoke(hitInfo.collider);
                }

                StartCoroutine(MeleeRoutine(targetDirection));
            }

            protected IEnumerator MeleeRoutine(Vector3 targetDirection)
            {
                Vector3 originalPosition = transform.position;
                //Vector3 offset = new Vector3(0, 0, 1f);
                float offset = 1f;
                float duration = 0.5f;

                transform.position = originalPosition + targetDirection * offset;

                yield return new WaitForSeconds(duration);

                transform.position = originalPosition;
            }
        }
    }
}