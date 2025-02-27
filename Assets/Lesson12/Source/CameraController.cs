using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _pitchAnchor;
    [SerializeField] private Transform _yawAnchor;
    [SerializeField] private float _sensitivity;

    private float _pitch = 0f;
    private float _yaw = 20f;

    private Vector2 _lookInput;

    // Start is called before the first frame update
    void Start()
    {
        InputController.OnLookInput += LookHandler;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        _pitchAnchor.localRotation = Quaternion.Euler(0f, _pitch, 0f);
        _yawAnchor.localRotation = Quaternion.Euler(_yaw, 0f, 0f);
    }

    private void LookHandler(Vector2 lookInput)
    {
        lookInput *= _sensitivity;
        _pitch += lookInput.x;
        _yaw += lookInput.y;
    }
}
