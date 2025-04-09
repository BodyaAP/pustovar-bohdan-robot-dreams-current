using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyLesson12
{
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
            _pitch -= _lookInput.y * _sensitivity * Time.deltaTime;
            _yaw += _lookInput.x * _sensitivity * Time.deltaTime;

            _pitchAnchor.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
            _yawAnchor.localRotation = Quaternion.Euler(0f, _yaw, 0f);
        }

        private void LookHandler(Vector2 lookInput)
        {
            //lookInput *= _sensitivity;
            //_pitch += lookInput.x;
            //_yaw += lookInput.y;

            _lookInput = lookInput;
        }
    }
}