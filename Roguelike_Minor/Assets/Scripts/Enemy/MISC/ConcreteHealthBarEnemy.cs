using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Core;

namespace Game.Enemy
{
    public class ConcreteHealthBarEnemy : HealthBar
    {
        [SerializeField] Slider slider;

        private void Start()
        {
            UpdateVisibility();
        }

        public override void UpdateHealthBar(float percentage)
        {
            slider.value = percentage;
            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            if (slider.value > 0.99f) 
            {
                if (slider.gameObject.activeSelf)
                {
                    slider.gameObject.SetActive(false);
                }
            }
            else if (!slider.gameObject.activeSelf)
            {
                slider.gameObject.SetActive(true);
            }
        }
    }
}
