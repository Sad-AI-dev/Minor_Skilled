using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game.Core.GameSystems {
    public class UIMoneyManager : MonoBehaviour
    {
        [SerializeField] UIManager uiManager;
        [SerializeField] TextMeshProUGUI moneyNumber;

        private void Start()
        {
            uiManager.agent.stats.onMoneyChanged += UpdateMoney;
        }

        void UpdateMoney(int newNumber)
        {
            moneyNumber.text = newNumber.ToString();
        }
    }
}
