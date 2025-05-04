using System;
using System.Collections.Generic;
using System.IO;
using MyLesson19.StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace MyLesson19
{
    namespace AudioSystem.SaveSystem
    {
        /// <summary>
        /// Implementation of ISaveService abstraction
        /// Implemented using json file
        /// Inherits GlobalMonoServiceBase, so it is a persistant service
        /// </summary>
        public class SaveService : GlobalMonoServiceBase, ISaveService
        {
            [SerializeField] private string _savePath;
            [SerializeField] private string _saveFile;

            private SaveData _saveData;

            public ref SaveData SaveData => ref _saveData;

            public override Type Type { get; } = typeof(ISaveService);

            protected override void Awake()
            {
                base.Awake();

                LoadAll();
            }

            protected override void OnDestroy()
            {
                SaveAll();

                base.OnDestroy();
            }

            public void SaveAll()
            {
                // File and Directory static classes operate in absolute paths
                // So in order to use relative path, it needs to be combined here with Application.persistantDataPath
                string path = Path.Combine(Application.persistentDataPath, _savePath);
                // If resulting directory does not exist, create it
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // combine directory path with file name in order to have complete file path
                string filePath = Path.Combine(path, _saveFile);
                // If file does not exist, create it
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Close();
                }

                // Write to file SaveData structure, serialized into Json by JsonUtility static class
                File.WriteAllText(filePath, JsonUtility.ToJson(_saveData, true));
            }

            public void LoadAll()
            {
                // File and Directory static classes operate in absolute paths
                // So in order to use relative path, it needs to be combined here with Application.persistantDataPath
                string path = Path.Combine(Application.persistentDataPath, _savePath);
                // If resulting directory does not exist, create it
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    return;
                }

                // combine directory path with file name in order to have complete file path
                string filePath = Path.Combine(path, _saveFile);
                // If file does not exist, create it, and set a default value to sound data
                if (!File.Exists(filePath))
                {
                    _saveData.soundData = SoundSaveData.Default;
                    return;
                }

                // If file does exist, use JsonUtility static class to deserialize content of the file
                _saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(filePath));
            }
        }
    }
}