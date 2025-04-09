using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyLesson12
{
    public class ExplosionController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _sphare;
        [SerializeField] private ParticleSystem _sparks;

        private ParticleSystem.MainModule _sphereMain;
        ParticleSystem.ShapeModule _sparksShape;

        public void ApplyRadius(float radius)
        {
            _sphereMain = _sphare.main;
            _sparksShape = _sphare.shape;

            _sphereMain.startSize = radius * 2f;
            _sparksShape.radius = radius * 4f / 3f;
        }

        public void Play()
        {
            _sphare.Play(true);
        }
    }
}