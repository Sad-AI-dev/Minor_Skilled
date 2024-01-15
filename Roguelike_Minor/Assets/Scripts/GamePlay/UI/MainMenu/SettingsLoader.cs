using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;
using Game.Core.GameSystems;

namespace Game {
    public class SettingsLoader : MonoBehaviour
    {
        [SerializeField] private SettingsSO settings;

        [Header("Volume Parameters")]
        [SerializeField] private RTPC masterVolumeParameter;
        [SerializeField] private RTPC musicVolumeParameter;
        [SerializeField] private RTPC sfxVolumeParameter;

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
            masterVolumeParameter.SetGlobalValue(settings.masterVolume);
            musicVolumeParameter.SetGlobalValue(settings.musicVolume);
            sfxVolumeParameter.SetGlobalValue(settings.sfxVolume);
        }
    }
}
