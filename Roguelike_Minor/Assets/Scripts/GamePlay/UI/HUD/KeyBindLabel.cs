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

        private void Start()
        {
            UpdateLabel();
            //listen to events
            EventBus<SettingsChanged>.AddListener(UpdateLabel);
        }

        private void UpdateLabel(SettingsChanged eventData = null)
        {
            label.text = settings.keyBinds[binding].ToString();
        }

        private void OnDestroy()
        {
            EventBus<SettingsChanged>.RemoveListener(UpdateLabel);
        }
    }
}
