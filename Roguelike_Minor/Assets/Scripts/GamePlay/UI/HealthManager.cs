using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game {
    public class HealthManager : MonoBehaviour
    {
        public UIManager uiManager;

        public TextMeshProUGUI healthNumber;
        public List<GameObject> healthBar;

        private void Update()
        {
            healthNumber.text = uiManager.agent.health.health.ToString();
        }
    }
}
