using MyLesson19.StateMachineSystem.ServiceLocatorSystem;

namespace MyLesson19
{
    namespace AudioSystem
    {
        /// <summary>
        /// Abstraction of a service, that can get and set volume to the designated AudioMixer
        /// </summary>
        public interface ISoundService : IService
        {
            void SetVolume(SoundType type, float volume);
            float GetVolume(SoundType type);
        }
    }
}