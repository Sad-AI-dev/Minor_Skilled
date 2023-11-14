using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using TMPro;
using UnityEngine.UI;

namespace Game {
    public class ConcreteHealthBarPlayer : HealthBar
    {
        [SerializeField] private UIManager uiManager;
        [SerializeField] private List<GameObject> healthBarStates;
        [SerializeField] private TextMeshProUGUI healthNumber;
        [SerializeField] private Slider healthBarSlider;

        private void Start()
        {
            UpdateHealthBar(1);
            healthNumber.text = GameStateManager.instance.player.stats.maxHealth.ToString();
        }

        public override void UpdateHealthBar(float percentage)
        {
            healthBarSlider.value = percentage;
            healthNumber.text = Mathf.CeilToInt(uiManager.agent.health.health).ToString();
        }
    }
}
