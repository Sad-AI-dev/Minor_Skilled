using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace Game {
    public class SettingSliderHandler : MonoBehaviour
    {
        [SerializeField] private UnityEvent<float> onValueChanged;

        [Header("Refs")]
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_InputField field;

        //=== Initialize ===
        private void Awake()
        {
            //listen to slider changes
            slider.onValueChanged.AddListener(OnSliderChanged);
            //listen to input field changes
            field.onEndEdit.AddListener(OnFieldChanged);
        }

        //==== manage ===
        public void SetValues(float value)
        {
            slider.value = value;
            field.text = string.Format("{0:0.##}", value);
        }

        //====== Handle Slider =======
        private void OnSliderChanged(float value)
        {
            field.text = string.Format("{0:0.##}", value);
            onValueChanged?.Invoke(value);
        }

        //====== Handle Input Field ======
        private void OnFieldChanged(string value)
        {
            //cast string to float
            if (float.TryParse(value, out float result))
            {
                float roundedValue = Mathf.Round(result * 100f) / 100f; //force 2 decimals max
                //update visuals
                field.text = roundedValue.ToString();
                slider.value = roundedValue;
                //notify others
                onValueChanged?.Invoke(roundedValue);
            }
        }
    }
}
