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
        public List<GameObject> healthBar;

        private void Start()
        {
            
        }

        private void Update()
        {
            healthNumber.text = uiManager.agent.health.health.ToString();


        }
    }
}
