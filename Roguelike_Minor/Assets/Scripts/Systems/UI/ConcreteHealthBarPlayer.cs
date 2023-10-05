using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Core;

namespace Game.Systems
{
    public class ConcreteHealthBarPlayer : HealthBar
    {
        public Slider healthbarSlider;

        private void Start()
        {
            healthbarSlider.maxValue = 1;
        }

        public override void UpdateHealthBar(float percentage)
        {
            healthbarSlider.value = percentage;
        }
    }
}
