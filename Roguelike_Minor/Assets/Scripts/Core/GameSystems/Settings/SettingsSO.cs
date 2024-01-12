using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Data;

namespace Game.Core.GameSystems {
    //all unique inputs used for keybinds
    public enum InputBinding { 
        forward, left, backward, right, jump, //movement binds
        primary, secondary, utility, special, //ability binds
        interact, openInventory, dropItem //interaction / inventory binds
    }

    [CreateAssetMenu(fileName = "settings", menuName = "ScriptableObjects/Test/SettingsTest")]
    public class SettingsSO : ScriptableObject
    {
        [HideInInspector] public bool initialized = false; //has settings file been generated?

        [Header("Graphics")]
        public bool fullScreen;

        [Header("Sensitivity")]
        public float mouseSensitivity;

        [Header("KeyBinds")]
        public UnityDictionary<InputBinding, KeyCode> keyBinds;

        [Header("Volume")]
        public float masterVolume;
        public float musicVolume;
        public float sfxVolume;
    }
}
