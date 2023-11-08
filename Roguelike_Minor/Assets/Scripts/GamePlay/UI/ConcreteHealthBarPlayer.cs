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
            healthNumber.text = uiManager.agent.health.health.ToString();
        }

       /* public override void UpdateHealthBar(float percentage)
        {
            healthBarStates[5].SetActive(false);
            healthBarStates[4].SetActive(false);
            healthBarStates[3].SetActive(false);
            healthBarStates[2].SetActive(false);
            healthBarStates[1].SetActive(false);
            healthBarStates[0].SetActive(false);

            if (percentage > 0.8f)
            {
                healthBarStates[5].SetActive(true);
            }
            else if (percentage > 0.6f)
            {
                healthBarStates[4].SetActive(true);
            }
            else if (percentage > 0.4f)
            {
                healthBarStates[3].SetActive(true);
            }
            else if (percentage > 0.2f)
            {
                healthBarStates[2].SetActive(true);
            }
            else if (percentage > 0)
            {
                healthBarStates[1].SetActive(true);
            }
            else
            {
                healthBarStates[0].SetActive(true);
            }

            healthNumber.text = uiManager.agent.health.health.ToString();
        }*/
    }
}
