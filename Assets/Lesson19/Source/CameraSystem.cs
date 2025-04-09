using UnityEngine;

namespace MyLesson19
{
    namespace Dummies
    {
        public class CameraSystem : MonoBehaviour
        {
            [SerializeField] private Camera _camera;

            public Camera Camera => _camera;
        }
    }
}