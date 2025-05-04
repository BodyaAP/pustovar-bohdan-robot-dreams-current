using MyLesson19.StateMachineSystem.ServiceLocatorSystem;

namespace MyLesson19
{
    namespace AudioSystem.SaveSystem
    {
        /// <summary>
        /// Abstraction of a service, which you can issue command to save data, load data and have access to data
        /// </summary>
        public interface ISaveService : IService
        {
            void SaveAll();
            void LoadAll();
            ref SaveData SaveData { get; }
        }
    }
}