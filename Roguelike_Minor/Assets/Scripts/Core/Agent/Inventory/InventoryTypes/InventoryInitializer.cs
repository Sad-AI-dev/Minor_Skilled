using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Data;

namespace Game.Core {
    [RequireComponent(typeof(Agent))]
    public class InventoryInitializer : MonoBehaviour
    {
        public UnityDictionary<ItemDataSO, int> itemsToGive;

        private void Start()
        {
            StartCoroutine(GiveItems(GetComponent<Agent>()));
        }

        private IEnumerator GiveItems(Agent agent)
        {
            yield return null; //wait a frame
            foreach (KeyValuePair<ItemDataSO, int> items in itemsToGive)
            {
                for (int i = 0; i < items.Value; i++)
                {
                    agent.inventory.TryAssignItem(items.Key);
                }
            }
        }
    }
}
