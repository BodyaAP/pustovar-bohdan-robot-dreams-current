using UnityEngine;

namespace MyLesson19
{
    namespace BehaviourTreeSystem
    {
        public interface INavPointProvider
        {
            Vector3 GetPoint();
        }
    }
}