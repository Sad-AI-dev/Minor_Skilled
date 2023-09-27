using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    public class HealthBarTest : HealthBar
    {
        public override void UpdateHealthBar(float percentage)
        {
            Debug.Log("Health is now at " + Mathf.RoundToInt(percentage * 100) + "%");
        }
    }
}
