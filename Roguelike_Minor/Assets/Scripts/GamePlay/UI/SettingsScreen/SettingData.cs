using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace Game {
    public class SettingData : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string title;
        [TextArea] public string description;

        [Header("Refs")]
        [SerializeField] private TMP_Text titleField;

        //actions
        public Action<SettingData> onHover;
        public Action<SettingData> onEndHover;

        //==== setup ====
        private void Awake()
        {
            if (titleField.text == "") { titleField.text = title; }
        }

        //=========== Handle Pointer Events ============
        public void OnPointerEnter(PointerEventData eventData)
        {
            onHover?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onEndHover?.Invoke(this);
        }
    }
}
