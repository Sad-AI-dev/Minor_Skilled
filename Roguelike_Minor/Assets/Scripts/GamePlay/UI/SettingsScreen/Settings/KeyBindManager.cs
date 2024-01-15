using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.GameSystems;

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
            screen.dirty = true; //make sure to save
        }

        //======= Save Setting =========
        public void SaveSetting(KeyBindSetting setting)
        {
            if (IsIncompatibleKeyBind(setting, out List<KeyBindSetting> incompatibles))
            {
                incompatibles.ForEach((KeyBindSetting incompatible) => incompatible.SetIncompatible(true));
                setting.SetIncompatible(true);
            }
            else
            { //no confilcts. save new keybind
                if (setting.isIncompatible) { setting.SetIncompatible(false); }
                //save to SO
                screen.settings.keyBinds[setting.binding] = setting.currentCode;
                //mark dirty
                screen.dirty = true;
            }
            //check for incompatibles
            IncompatibleCheck();
        }

        private bool IsIncompatibleKeyBind(KeyBindSetting setting, out List<KeyBindSetting> incompatibleBindings)
        {
            //get all keybinds with the same key code
            incompatibleBindings = new List<KeyBindSetting>();
            //search for non compatible
            foreach (KeyBindSetting otherSetting in keyBindSettings)
            {
                if (otherSetting != setting && //make sure not checking self
                    otherSetting.currentCode == setting.currentCode && //check if codes are the same
                    !setting.compatibleKeyBinds.Contains(otherSetting) //compatible check
                )
                {
                    incompatibleBindings.Add(otherSetting);
                }
            }
            //return result
            return incompatibleBindings.Count > 0;
        }

        //============ handle incompatible keybinds ===============
        private void IncompatibleCheck()
        {
            foreach (KeyBindSetting setting in keyBindSettings)
            {
                if (setting.isIncompatible)
                {
                    if (!IsIncompatibleKeyBind(setting, out List<KeyBindSetting> incompatibles))
                    {
                        //setting is no longer incompatible, save
                        setting.SetIncompatible(false);
                        screen.settings.keyBinds[setting.binding] = setting.currentCode;
                    }
                }
            }
        }
    }
}
