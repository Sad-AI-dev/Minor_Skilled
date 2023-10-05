using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Game.Core;

namespace Game.Systems
{

    public class HealthManager : MonoBehaviour
    {
        public UIManager uiManager;

        public TextMeshProUGUI healthNumber;
        public Slider healthBar;

        private void Start()
        {
            healthBar.maxValue = uiManager.agent.stats.maxHealth;
        }

        private void Update()
        {
            healthNumber.text = uiManager.agent.health.health.ToString();
        }
    }
}
