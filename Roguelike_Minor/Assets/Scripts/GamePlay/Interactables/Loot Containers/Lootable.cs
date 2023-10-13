using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.GameSystems;
using TMPro;

namespace Game {
    public class Lootable : MonoBehaviour
    {
        [SerializeField] private int price;
        [SerializeField] private LootTableSO lootTable;

        [Header("Technical")]
        [SerializeField] private TMP_Text priceLabel;

        private void Start()
        {
            priceLabel.text = "$" + price;
        }

        public void TryPurchase(Interactor interactor)
        {
            Agent agent = interactor.GetComponent<Agent>();
            if (agent.stats.Money >= price)
            {
                agent.stats.Money -= price;
                DropLoot();
                Destroy(gameObject);
            }
        }

        private void DropLoot()
        {
            GameObject obj = Instantiate(lootTable.GetLoot());
            obj.transform.SetPositionAndRotation(transform.position + transform.forward, GetRandomRotation());
        }

        private Quaternion GetRandomRotation()
        {
            return Quaternion.Euler(new Vector3(0, Random.Range(0, 360f), 0));
        }
    }
}
