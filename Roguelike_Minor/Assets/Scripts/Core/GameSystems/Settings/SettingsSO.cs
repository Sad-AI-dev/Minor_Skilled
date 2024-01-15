using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Data;

namespace Game.Core.GameSystems {
    //all unique inputs used for keybinds
    public enum InputBinding { 
        Forward, Left, Backward, Right, Jump, //movement binds
        Primary, Secondary, Utility, Special, //ability binds
        Interact, OpenInventory, DropItem //interaction / inventory binds
    }

    [CreateAssetMenu(fileName = "settings", menuName = "ScriptableObjects/Test/SettingsTest")]
    public class SettingsSO : ScriptableObject
    {
        [HideInInspector] public bool initialized = false; //has settings file been generated?

        [Header("Graphics")]
        public bool fullScreen;
        public bool useVsync;
        public bool limitFrameRate;
        public int targetFrameRate = 60;

        [Header("Sensitivity")]
        public float mouseSensitivity;

        [Header("KeyBinds")]
        public UnityDictionary<InputBinding, KeyCode> keyBinds;

        [Header("Volume")]
        public float masterVolume;
        public float musicVolume;
        public float sfxVolume;

        public void SetDefaultValues()
        {
            //graphics
            fullScreen = true;
            targetFrameRate = 60;
            //controls
            mouseSensitivity = 0.5f; //PLACEHOLDER!
            keyBinds = new UnityDictionary<InputBinding, KeyCode>
            {
                //movement
                new KeyValuePair<InputBinding, KeyCode>(InputBinding.Forward, KeyCode.W),
                new KeyValuePair<InputBinding, KeyCode>(InputBinding.Left, KeyCode.A),
                new KeyValuePair<InputBinding, KeyCode>(InputBinding.Backward, KeyCode.S),
                new KeyValuePair<InputBinding, KeyCode>(InputBinding.Right, KeyCode.D),
                new KeyValuePair<InputBinding, KeyCode>(InputBinding.Jump, KeyCode.Space),
                //abilities
                new KeyValuePair<InputBinding, KeyCode>(InputBinding.Primary, KeyCode.Mouse0),
                new KeyValuePair<InputBinding, KeyCode>(InputBinding.Secondary, KeyCode.Mouse1),
                new KeyValuePair<InputBinding, KeyCode>(InputBinding.Special, KeyCode.Q),
                new KeyValuePair<InputBinding, KeyCode>(InputBinding.Utility, KeyCode.LeftShift),
                //interaction
                new KeyValuePair<InputBinding, KeyCode>(InputBinding.Interact, KeyCode.E),
                new KeyValuePair<InputBinding, KeyCode>(InputBinding.OpenInventory, KeyCode.Tab),
                new KeyValuePair<InputBinding, KeyCode>(InputBinding.DropItem, KeyCode.E)
            };
            //audio
            masterVolume = 50f;
            musicVolume = 50f;
            sfxVolume = 50f;
        }
    }
}
