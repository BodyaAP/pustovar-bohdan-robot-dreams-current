using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyLesson14
{
    public class CameraSystem : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        public Camera Camera => _camera;
    }
}