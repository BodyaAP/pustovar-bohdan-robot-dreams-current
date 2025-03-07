using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalBillboard : BillboardBase
{
    private Transform _cameraTransform;
    private Camera _camera;
    private Transform _transform;

    public override void SetCamera(Camera camera)
    {
        _transform = transform;
        _camera = camera;
        _cameraTransform = _camera.transform;
    }

    private void LateUpdate()
    {
        Vector3 direction = _camera.transform.position - _transform.position;
        direction = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
        _transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }
}
