using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game {
    public class SettingData : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string title;
        [TextArea] public string description;

        //actions
        public Action<SettingData> onHover;
        public Action<SettingData> onEndHover;

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
