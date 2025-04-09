namespace MyLesson19
{
    namespace StateMachineSystem
    {
        public interface IStateCondition
        {
            byte State { get; }

            bool Invoke();
        }
    }
}