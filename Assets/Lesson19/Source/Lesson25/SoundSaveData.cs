using System;

namespace MyLesson19
{
    namespace AudioSystem.SaveSystem
    {
        /// <summary>
        /// Save data part related to sound
        /// Default is a static readonly struct that has default values
        /// </summary>
        [Serializable]
        public struct SoundSaveData
        {
            public static readonly SoundSaveData Default = new SoundSaveData()
                { masterVolume = 0.5f, sfxVolume = 0.5f, ambienceVolume = 0.5f };

            public float masterVolume;
            public float sfxVolume;
            public float ambienceVolume;
        }
    }
}