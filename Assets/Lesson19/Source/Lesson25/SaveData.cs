using System;

namespace MyLesson19
{
    namespace AudioSystem.SaveSystem
    {
        /// <summary>
        /// General structure for saved data
        /// Contains structs of specific save data, separated in thematic structs
        /// </summary>
        [Serializable]
        public struct SaveData
        {
            // Data related to sound
            public SoundSaveData soundData;

            // Data related to localization
            public LocalizationSaveData localizationData;
        }
    }
}