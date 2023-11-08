using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game.Core
{
    public class EffectIconElement : MonoBehaviour
    {
        public Image icon;
        public TMP_Text stackLabel;

        private string baseText;

        public void Initialize()
        {
            baseText = stackLabel.text;
            //initialize transform
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
        }

        public void SetStacks(int stacks)
        {
            stackLabel.text = baseText + stacks.ToString();
        }

        public void ResetToDefault()
        {
            icon.sprite = null;
            stackLabel.text = baseText;
        }
    }
}
