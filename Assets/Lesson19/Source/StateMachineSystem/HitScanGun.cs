using System;
using System.Collections;
using Shooting;
using MyLesson19.StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace MyLesson19
{
    namespace StateMachineSystem
    {
        public class HitScanGun : MonoBehaviour
        {
            public event Action<Collider> OnHit;
            public event Action OnShot;

            [SerializeField] protected GunAimer _aimer;
            [SerializeField] protected HitscanShotAspect _shotPrefab;
            [SerializeField] protected Transform _muzzleLeftTransform;
            [SerializeField] protected Transform _muzzleRightTransform;
            [SerializeField] protected float _decaySpeed;
            [SerializeField] protected Vector3 _shotScale;
            [SerializeField] protected float _shotRadius;
            [SerializeField] protected float _shotVisualDiameter;
            [SerializeField] protected string _tilingName;
            [SerializeField] protected float _range;
            [SerializeField] protected LayerMask _layerMask;

            [SerializeField] protected Transform _weaponTransform;

            [SerializeField] private GameObject _muzzleFlashPrefab;
            [SerializeField] private bool _unparentMuzzleFlash;
            [SerializeField] private GameObject _impactVfx;
            [SerializeField] private float _impactVfxLifetime = 5f;
            [SerializeField] private float _impactVfsSpawnOffset = 0.1f;

            protected int _tilingId;
            private bool isLeftGunBarrel = true;

            protected InputController _inputController;

            protected virtual void Start()
            {
                _tilingId = Shader.PropertyToID(_tilingName);
            }

            protected void OnEnable()
            {
                if (_inputController == null)
                    _inputController = ServiceLocator.Instance.GetService<InputController>();
                _inputController.OnPrimaryInput += PrimaryInputHandler;
            }

            protected void OnDisable()
            {
                _inputController.OnPrimaryInput -= PrimaryInputHandler;
            }

            protected virtual void PrimaryInputHandler()
            {
                Transform muzzle = isLeftGunBarrel ? _muzzleLeftTransform : _muzzleRightTransform;

                //Vector3 muzzlePosition = _muzzleTransform.position;
                //Vector3 muzzleForward = _muzzleTransform.forward;
                Ray ray = new Ray(muzzle.position, muzzle.forward);
                Vector3 hitPoint = muzzle.position + muzzle.forward * _range;

                if (_muzzleFlashPrefab != null)
                {
                    GameObject muzzleFlashInstance =
                        Instantiate(_muzzleFlashPrefab, muzzle.position, muzzle.rotation, muzzle.transform);

                    if (_unparentMuzzleFlash)
                    {
                        muzzleFlashInstance.transform.SetParent(null);
                    }

                    Destroy(muzzleFlashInstance, 2f);
                }

                if (Physics.SphereCast(ray, _shotRadius, out RaycastHit hitInfo, _range, _layerMask))
                {
                    Vector3 directVector = hitInfo.point - muzzle.position;
                    Vector3 rayVector = Vector3.Project(directVector, ray.direction);
                    hitPoint = muzzle.position + rayVector;

                    if (_impactVfx)
                    {
                        GameObject impactVfxInstance = Instantiate(_impactVfx,
                            hitInfo.point + (hitInfo.normal * _impactVfsSpawnOffset),
                            Quaternion.LookRotation(hitInfo.normal));
                        if (_impactVfxLifetime > 0)
                        {
                            Destroy(impactVfxInstance.gameObject, _impactVfxLifetime);
                        }
                    }

                    OnHit?.Invoke(hitInfo.collider);
                }

                HitscanShotAspect shot = Instantiate(_shotPrefab, hitPoint, muzzle.rotation);
                shot.distance = (hitPoint - muzzle.position).magnitude;
                shot.outerPropertyBlock = new MaterialPropertyBlock();

                isLeftGunBarrel = !isLeftGunBarrel;

                StartCoroutine(ShotRoutine(shot));
                //StartCoroutine(RecoilRoutine());

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

            protected IEnumerator RecoilRoutine()
            {
                Debug.Log("Recoil");
                float offset = 0.5f;
                float duration = 0.15f;

                Vector3 originalPosition = _weaponTransform.position;
                Quaternion originalRotation = _weaponTransform.rotation;

                Vector3 direction = _weaponTransform.forward.normalized;

                _weaponTransform.position = originalPosition + direction * offset;
                _weaponTransform.rotation = originalRotation * Quaternion.Euler(-35f, 0, 0);

                yield return new WaitForSeconds(duration);

                _weaponTransform.position = originalPosition;
                _weaponTransform.rotation = originalRotation;
            }

            //protected IEnumerator RecoilRoutine()
            //{
            //    Debug.Log("Recoil");
            //    float offset = 0.2f; // не надто велике значення
            //    float duration = 0.15f;

            //    Vector3 originalPosition = _weaponTransform.position;
            //    Quaternion originalRotation = _weaponTransform.rotation;

            //    Vector3 direction = _weaponTransform.forward.normalized;
            //    Vector3 recoilPosition = originalPosition + direction * offset;
            //    Quaternion recoilRotation = originalRotation * Quaternion.Euler(35f, 0f, 0f);

            //    float elapsed = 0f;

            //    while (elapsed < duration)
            //    {
            //        float t = elapsed / duration;
            //        _weaponTransform.position = Vector3.Lerp(recoilPosition, originalPosition, t);
            //        _weaponTransform.rotation = Quaternion.Slerp(recoilRotation, originalRotation, t);
            //        elapsed += Time.deltaTime;
            //        yield return null;
            //    }

            //    // На всяк випадок — фінальне встановлення точних значень
            //    _weaponTransform.position = originalPosition;
            //    _weaponTransform.rotation = originalRotation;
            //}
        }
    }
}