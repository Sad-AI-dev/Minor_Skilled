using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.GameSystems {
    public class SettingsSaveLoad
    {
        //vars
        private readonly string filePath;

        //ctor
        public SettingsSaveLoad(string fileName)
        {
            filePath = Application.persistentDataPath + "/" + fileName;
        }

        //============ Load Data =============
        public void LoadSettings(SettingsSO settingsSO)
        {
            //is there any save data?
            if (File.Exists(filePath))
            {
                string fileContents = File.ReadAllText(filePath);
                //overwrite SO object
                JsonUtility.FromJsonOverwrite(fileContents, settingsSO);
            }
            else {
                //create default values
                settingsSO.SetDefaultValues();
                settingsSO.initialized = true;
            }
        }

        //============ Save Data =============
        public void SaveSettings(SettingsSO settingsSO)
        {
            if (!settingsSO.initialized) { settingsSO.initialized = true; }
            //convert to json string
            string jsonContents = JsonUtility.ToJson(settingsSO);
            //write to file
            File.WriteAllText(filePath, jsonContents);
        }
    }
}
