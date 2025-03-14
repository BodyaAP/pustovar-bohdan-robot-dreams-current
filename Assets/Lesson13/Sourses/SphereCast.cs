using Shooting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCast : MonoBehaviour
{
    public event Action<Collider> OnHit;
    public event Action OnShot;
    
    [SerializeField] private HitscanShotAspect _shotPrefab;
    [SerializeField] private Transform _muzzleLeftTransform;
    [SerializeField] private Transform _muzzleRightTransform;
    [SerializeField] private float _decaySpeed;
    [SerializeField] private Vector3 _shotScale;
    [SerializeField] private float _shotRadius;
    [SerializeField] private float _shotVisualDiameter;
    [SerializeField] private string _tilingName;
    [SerializeField] private float _range;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private GunAimer _aimer;

    [SerializeField] private GameObject _muzzleFlashPrefab;
    [SerializeField] private bool _unparentMuzzleFlash;
    [SerializeField] private GameObject _impactVfx;
    [SerializeField] private float _impactVfxLifetime = 5f;
    [SerializeField] private float _impactVfsSpawnOffset = 0.1f;

    private int _tilingId;
    private bool isLeftGunBarrel = true;

    // Start is called before the first frame update
    void Start()
    {
        _tilingId = Shader.PropertyToID(_tilingName);
    }

    private void OnEnable()
    {
        InputController.OnSphereCastInput += SphereCastInputHandler;
    }

    private void OnDisable()
    {
        InputController.OnSphereCastInput -= SphereCastInputHandler;
    }

    private void SphereCastInputHandler()
    {
        //Vector3 muzzlePosition = isLeftGunBarrel ? _muzzleLeftTransform.position : _muzzleRightTransform.position; //_muzzleTransform.position;
        //Vector3 muzzleForward = isLeftGunBarrel ? _muzzleLeftTransform.forward : _muzzleRightTransform.forward;

        Transform muzzle = isLeftGunBarrel ? _muzzleLeftTransform : _muzzleRightTransform;

        //Quaternion muzzleRotation = isLeftGunBarrel ? _muzzleLeftTransform.rotation : _muzzleRightTransform.rotation;
        Ray ray = new Ray(muzzle.position, muzzle.forward);
        Vector3 hitPoint = muzzle.position + muzzle.forward * _range;

        if (_muzzleFlashPrefab != null)
        {
            GameObject muzzleFlashInstance = Instantiate(_muzzleFlashPrefab, muzzle.position, muzzle.rotation, muzzle.transform);

            if (_unparentMuzzleFlash)
            {
                muzzleFlashInstance.transform.SetParent(null);
            }

            Destroy(muzzleFlashInstance, 2f);
        }

        if (Physics.SphereCast(ray, _shotRadius, out RaycastHit hitInfo, _range, _layerMask))
        {
            Vector3 directVector = hitInfo.point - muzzle.position; //_muzzleTransform.position;
            Vector3 rayVector = Vector3.Project(directVector, ray.direction);
            hitPoint = muzzle.position + rayVector;

            if (_impactVfx)
            {
                GameObject impactVfxInstance = Instantiate(_impactVfx, hitInfo.point + (hitInfo.normal * _impactVfsSpawnOffset), Quaternion.LookRotation(hitInfo.normal));
                if (_impactVfxLifetime > 0)
                {
                    Destroy(impactVfxInstance.gameObject, _impactVfxLifetime);
                }
            }

            OnHit?.Invoke(hitInfo.collider);
        }

        HitscanShotAspect shot = Instantiate(_shotPrefab, hitPoint, muzzle.rotation /*_muzzleTransform.rotation*/);
        shot.distance = (hitPoint - muzzle.position /*_muzzleTransform.position*/).magnitude;
        shot.outerPropertyBlock = new MaterialPropertyBlock();

        //isLeftGunBarrel = isLeftGunBarrel ? false : true;
        isLeftGunBarrel = !isLeftGunBarrel;

        StartCoroutine(ShotRoutine(shot));

        OnShot?.Invoke();
    }

    private IEnumerator ShotRoutine(HitscanShotAspect shot)
    {
        float interval = _decaySpeed * Time.deltaTime; ;

        while (shot.distance >= interval)
        {
            EvaluateShot(shot);
            yield return null;
            shot.distance -= interval;
            interval = _decaySpeed * Time.deltaTime;
        }

        Destroy(shot.gameObject);
    }

    private void EvaluateShot(HitscanShotAspect shot)
    {
        shot.Transform.localScale = new Vector3(_shotScale.x, _shotScale.y, shot.distance * 0.5f);
        Vector4 tiling = Vector4.one;
        tiling.y = shot.distance *0.5f / _shotVisualDiameter;
        shot.outerPropertyBlock.SetVector(_tilingId, tiling);
        shot.Outer.SetPropertyBlock(shot.outerPropertyBlock);
    }
}
