using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Game.Core.GameSystems;

namespace Game {
    public class KeyBindSetting : MonoBehaviour
    {
        [Header("Settings")]
        public KeyCode defaultCode;
        public InputBinding binding;
        public List<KeyBindSetting> compatibleKeyBinds;
        public UnityEvent<KeyBindSetting> onSetCode;

        [Header("Refs")]
        [SerializeField] private KeyBindManager keyBindManager;
        [SerializeField] private TMP_Text bindLabel;

        [Header("Visual Settings")]
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private Color bindingColor = Color.yellow;
        [SerializeField] private Color incompatibleColor = Color.red;

        //vars
        [HideInInspector] public bool isIncompatible;
        [HideInInspector] public KeyCode currentCode;

        //====== Manage State ==========
        public void InitializeCode(KeyCode code)
        {
            currentCode = code;
            UpdateVisuals();
        }

        //======= Manage Visuals ========
        private void UpdateVisuals()
        {
            bindLabel.text = currentCode.ToString();
            bindLabel.color = isIncompatible ? incompatibleColor : defaultColor;
        }

        //======= Reset ============
        public void ResetToDefault()
        {
            currentCode = defaultCode;
            //notify others
            onSetCode?.Invoke(this);
            //update visuals
            UpdateVisuals();
        }
    }
}
