using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game {
    [RequireComponent(typeof(SettingsScreen))]
    public class SettingApplier : MonoBehaviour
    {
        [Header("Graphic Refs")]
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private Toggle vSyncToggle;
        [Space(10f)]
        [SerializeField] private Toggle limitFrameRateToggle;
        [SerializeField] private GameObject targetFrameRateHolder;
        [SerializeField] private TMP_InputField targetFrameRateField;

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
            //vsync
            vSyncToggle.isOn = screen.settings.useVsync;
            //target frame rate
            limitFrameRateToggle.isOn = screen.settings.limitFrameRate;
            targetFrameRateHolder.SetActive(screen.settings.limitFrameRate);
            targetFrameRateField.text = screen.settings.targetFrameRate.ToString();
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
            //set screen dirty
            screen.dirty = true;
        }

        public void SetVsync(bool mode)
        {
            QualitySettings.vSyncCount = mode ? 1 : 0;
            screen.settings.useVsync = mode;
            //set screen dirty
            screen.dirty = true;
        }

        //fps
        public void SetLimitFPS(bool mode)
        {
            Application.targetFrameRate = mode ? screen.settings.targetFrameRate : -1; //-1 is default value for no limit
            screen.settings.limitFrameRate = mode;
            targetFrameRateHolder.SetActive(mode);
            //set screen dirty
            screen.dirty = true;
        }

        public void SetTargetFPS(string input)
        {
            if (int.TryParse(input, out int result))
            {
                if (screen.settings.limitFrameRate) { Application.targetFrameRate = result; }
                screen.settings.targetFrameRate = result;
                //set screen dirty
                screen.dirty = true;
            }
        }

        //=== Sensitivity ===
        public void SetSensitivity(float sensitivity)
        {
            screen.settings.mouseSensitivity = sensitivity;
            //set screen dirty
            screen.dirty = true;
        }

        //=== Audio ===
        public void SetMasterVolume(float volume)
        {
            screen.settings.masterVolume = volume;
            //set screen dirty
            screen.dirty = true;
        }

        public void SetMusicVolume(float volume)
        {
            screen.settings.musicVolume = volume;
            //set screen dirty
            screen.dirty = true;
        }

        public void SetSFXVolume(float volume)
        {
            screen.settings.sfxVolume = volume;
            //set screen dirty
            screen.dirty = true;
        }
    }
}
