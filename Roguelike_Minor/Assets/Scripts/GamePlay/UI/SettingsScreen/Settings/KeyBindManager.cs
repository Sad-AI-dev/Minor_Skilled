using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class KeyBindManager : MonoBehaviour
    {
        [Header("Refs")]
        public SettingsScreen screen;
        [SerializeField] private RectTransform keyBindSettingHolder;

        //vars
        private KeyBindSetting[] keyBindSettings;

        //====== Initialize =====
        private void Awake()
        {
            InitializeKeyBindSettings();
            LoadCurrentKeyBinds();
        }

        private void InitializeKeyBindSettings()
        {
            keyBindSettings = keyBindSettingHolder.GetComponentsInChildren<KeyBindSetting>();
        }

        private void LoadCurrentKeyBinds()
        {
            if (screen.settings.initialized)
            {
                for (int i = 0; i < keyBindSettings.Length; i++)
                {
                    keyBindSettings[i].InitializeCode(screen.settings.keyBinds[keyBindSettings[i].binding]);
                }
            }
            //no save file, generate default key binds
            else { RestoreDefaultKeyBinds(); }
        }

        //========= Restore Keybinds ============
        public void RestoreDefaultKeyBinds()
        {
            System.Array.ForEach(keyBindSettings, (KeyBindSetting setting) => setting.ResetToDefault());
        }

        //======= Save Setting =========
        public void SaveSetting(KeyBindSetting setting)
        {
            
            //mark dirty
            screen.dirty = true;
        }

        private bool IsCompatibleKeyBind(KeyBindSetting setting)
        {
            //get all keybinds with the same key code
            List<KeyBindSetting> incompatibleBindings = new List<KeyBindSetting>();
            return true; //del me
        }
    }
}
