using System.Collections;

namespace MyLesson19
{
    namespace StateMachineSystem
    {
        public interface ICoroutineExecutable
        {
            void ExecuteCoroutine(IEnumerator routine);
        }
    }
}