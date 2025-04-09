using UnityEngine;

namespace MyLesson19
{
    namespace PhysX
    {
        public interface ITargetable
        {
            public Transform TargetPivot { get; }
        }
    }
}