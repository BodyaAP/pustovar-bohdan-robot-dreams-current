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
        Vector3 muzzlePosition = isLeftGunBarrel ? _muzzleLeftTransform.position : _muzzleRightTransform.position; //_muzzleTransform.position;
        Vector3 muzzleForward = isLeftGunBarrel ? _muzzleLeftTransform.forward : _muzzleRightTransform.forward;
        Quaternion muzzleRotation = isLeftGunBarrel ? _muzzleLeftTransform.rotation : _muzzleRightTransform.rotation;
        Ray ray = new Ray(muzzlePosition, muzzleForward);
        Vector3 hitPoint = muzzlePosition + muzzleForward * _range;

        if (Physics.SphereCast(ray, _shotRadius, out RaycastHit hitInfo, _range, _layerMask))
        {
            Vector3 directVector = hitInfo.point - muzzlePosition; //_muzzleTransform.position;
            Vector3 rayVector = Vector3.Project(directVector, ray.direction);
            hitPoint = muzzlePosition + rayVector;

            OnHit?.Invoke(hitInfo.collider);
        }

        HitscanShotAspect shot = Instantiate(_shotPrefab, hitPoint, muzzleRotation /*_muzzleTransform.rotation*/);
        shot.distance = (hitPoint - muzzlePosition /*_muzzleTransform.position*/).magnitude;
        shot.outerPropertyBlock = new MaterialPropertyBlock();

        isLeftGunBarrel = isLeftGunBarrel ? false : true;

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
