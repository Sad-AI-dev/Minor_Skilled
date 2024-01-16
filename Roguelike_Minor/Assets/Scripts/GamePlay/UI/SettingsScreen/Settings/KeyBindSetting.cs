using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Game.Core.GameSystems;
using System;

namespace Game {
    public class KeyBindSetting : MonoBehaviour
    {
        [Header("Settings")]
        public KeyCode defaultCode;
        public InputBinding binding;
        public List<KeyBindSetting> compatibleKeyBinds;
        public UnityEvent<KeyBindSetting> onSetCode;

        [Header("Refs")]
        [SerializeField] private TMP_Text bindLabel;

        [Header("Visual Settings")]
        [SerializeField] private string listeningString = "> <";
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private Color listeningColor = Color.yellow;
        [SerializeField] private Color incompatibleColor = Color.red;

        //vars
        [HideInInspector] public bool isIncompatible;
        [HideInInspector] public KeyCode currentCode;
        private bool listening;

        //====== Manage State ==========
        public void InitializeCode(KeyCode code)
        {
            currentCode = code;
            UpdateVisuals();
        }

        public void SetIncompatible(bool state)
        {
            if (isIncompatible != state)
            {
                isIncompatible = state;
                UpdateVisuals();
            }
        }

        //======= Manage Visuals ========
        private void UpdateVisuals()
        {
            bindLabel.text = currentCode.ToString();
            bindLabel.color = isIncompatible ? incompatibleColor : defaultColor;
        }

        //======== Listen For Input ============
        public void ListenForInput()
        {
            if (!listening)
            {
                listening = true;
                //setup visuals
                bindLabel.text = listeningString;
                bindLabel.color = listeningColor;
                //listen for input
                StartCoroutine(ListenCo());
            }
        }

        private IEnumerator ListenCo()
        {
            yield return null; //make sure not to listen to input needed to start listen
            yield return new WaitWhile(() => !Input.anyKeyDown);
            //input detected
            if (Input.GetKeyDown(KeyCode.Escape))
            { //cancel, return to default state
                InitializeCode(currentCode);
            }
            else
            {
                foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(code))
                    {
                        SetKeyCode(code);
                        break; //stop looking for key codes
                    }
                }
            }
            listening = false;
        }

        private void SetKeyCode(KeyCode code)
        {
            currentCode = code;
            //notify others
            onSetCode?.Invoke(this);
            //update visuals
            UpdateVisuals();
        }

        //======= Reset ============
        public void ResetToDefault()
        {
            SetKeyCode(defaultCode);
        }
    }
}
