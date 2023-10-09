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

        public override void UpdateHealthBar(float percentage)
        {
            slider.value = percentage;
        }
    }
}
