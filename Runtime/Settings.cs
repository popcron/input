using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popcron.Input
{
    [Serializable]
    public class Settings
    {
        private static Settings settings;

        private string path;

        [SerializeField]
        private int maxControllers = 4;

        private static Settings Instance
        {
            get
            {
                if (settings == null)
                {
                    string path = Directory.GetParent(Application.dataPath).FullName;
                    path = Path.Combine(path, "ProjectSettings", "InputSettings.asset");
                    bool exists = System.IO.File.Exists(path);
                    if (settings == null || !exists)
                    {
                        settings = new Settings(path);
                        if (File.Exists(path))
                        {
                            string json = File.ReadAllText(path);
                            JsonUtility.FromJsonOverwrite(json, settings);
                        }
                    }

                    return settings;
                }

                return settings;
            }
        }

        public static int MaxControllers
        {
            get
            {
                return Instance.maxControllers;
            }
            set
            {
                if (Instance.maxControllers != value)
                {
                    Instance.maxControllers = value;
                    Instance.Save();
                }
            }
        }

        private Settings(string path)
        {
            this.path = path;
        }

        private void Save()
        {
            string json = JsonUtility.ToJson(this, true);
            File.WriteAllText(path, json);
        }
    }
}