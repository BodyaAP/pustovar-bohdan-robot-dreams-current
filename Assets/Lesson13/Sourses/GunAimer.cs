using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyLesson13
{
    public class GunAimer : MonoBehaviour
    {
        [SerializeField] private Transform _gunTransform;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _rayDistance;
        [SerializeField] private LayerMask _rayMask;

        private Vector3 _hitPoint;

        public Vector3 AimPoint => _hitPoint;

        private void FixedUpdate()
        {
            Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
            _hitPoint = _cameraTransform.position + _cameraTransform.forward * _rayDistance;

            if (Physics.Raycast(ray, out RaycastHit hitInfo, _rayDistance, _rayMask))
            {
                _hitPoint = hitInfo.point;
            }

            _gunTransform.LookAt(_hitPoint);
        }
    }
}