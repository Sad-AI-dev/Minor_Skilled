using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.GameSystems;

namespace Game {
    public class SettingsLoader : MonoBehaviour
    {
        [SerializeField] private SettingsSO settings;

        private void Awake()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            SettingsSaveLoad serializer = new SettingsSaveLoad("settings.json");
            serializer.LoadSettings(settings);
            //apply loaded settings
            ApplySettings();
        }

        //========= Apply Settings ============
        private void ApplySettings()
        {
            ApplyGraphicsSettings();
            ApplyAudioSettings();
        }

        //==== Graphics ====
        private void ApplyGraphicsSettings()
        {
            //set fullscreen
            Screen.fullScreen = settings.fullScreen;
            //set vsync
            QualitySettings.vSyncCount = settings.useVsync ? 1 : 0;
            //set limit framerate
            Application.targetFrameRate = settings.limitFrameRate ? settings.targetFrameRate : -1;
        }

        //==== Audio ====
        private void ApplyAudioSettings()
        {
            //TODO apply audio settings
        }
    }
}
