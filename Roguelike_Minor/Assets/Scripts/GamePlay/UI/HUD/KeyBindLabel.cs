using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game.Core;
using Game.Core.GameSystems;

namespace Game
{
    public class KeyBindLabel : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private SettingsSO settings;
        [SerializeField] private TMP_Text label;

        [Header("Settings")]
        [SerializeField] private InputBinding binding;
        [SerializeField] private KeyBindLabelDisplaySO displaySettings;

        private void Start()
        {
            UpdateLabel();
            //listen to events
            EventBus<SettingsChanged>.AddListener(UpdateLabel);
        }

        private void UpdateLabel(SettingsChanged eventData = null)
        {
            label.text = GetKeyString();
        }
        private string GetKeyString()
        {
            KeyCode code = settings.keyBinds[binding];
            if (displaySettings.displayDataDict.ContainsKey(code))
            {
                return displaySettings.displayDataDict[code].replaceString.ToUpper();
            }
            else { return code.ToString().ToUpper(); }
        }

        private void OnDestroy()
        {
            EventBus<SettingsChanged>.RemoveListener(UpdateLabel);
        }
    }
}
