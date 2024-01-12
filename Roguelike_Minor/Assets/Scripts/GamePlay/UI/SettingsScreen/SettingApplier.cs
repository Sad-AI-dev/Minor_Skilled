using Game.Core.GameSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game {
    [RequireComponent(typeof(SettingsScreen))]
    public class SettingApplier : MonoBehaviour
    {
        [Header("Graphic Refs")]
        [SerializeField] private Toggle fullscreenToggle;

        [Header("Control Refs")]
        [SerializeField] private SettingSliderHandler sensitivitySlider;

        [Header("Audio Refs")]
        [SerializeField] private SettingSliderHandler masterVolume;
        [SerializeField] private SettingSliderHandler musicVolume;
        [SerializeField] private SettingSliderHandler sfxVolume;

        private SettingsScreen screen;

        private void Awake()
        {
            screen = GetComponent<SettingsScreen>();
            //load default values
            LoadDefaults();
        }

        //========== Set Default Values =========
        private void LoadDefaults()
        {
            LoadGraphicsValues();
            LoadControlValues();
            LoadAudioValues();
        }

        //== Graphics Values ==
        private void LoadGraphicsValues()
        {
            //fullscreen
            fullscreenToggle.isOn = screen.settings.fullScreen;
        }

        //== control values ==
        private void LoadControlValues()
        {
            sensitivitySlider.SetValues(screen.settings.mouseSensitivity);
        }

        //== Audio Values ==
        private void LoadAudioValues()
        {
            //set volumes
            masterVolume.SetValues(screen.settings.masterVolume);
            musicVolume.SetValues(screen.settings.musicVolume);
            sfxVolume.SetValues(screen.settings.sfxVolume);
        }

        //========== Apply Settings ============
        //=== Graphics ===
        public void SetFullScreen(bool mode)
        {
            screen.settings.fullScreen = mode;
            Screen.fullScreen = mode;
        }

        //=== Sensitivity ===
        public void SetSensitivity(float sensitivity)
        {
            screen.settings.mouseSensitivity = sensitivity;
        }

        //=== Audio ===
        public void SetMasterVolume(float volume)
        {
            screen.settings.masterVolume = volume;
        }

        public void SetMusicVolume(float volume)
        {
            screen.settings.musicVolume = volume;
        }

        public void SetSFXVolume(float volume)
        {
            screen.settings.sfxVolume = volume;
        }
    }
}
