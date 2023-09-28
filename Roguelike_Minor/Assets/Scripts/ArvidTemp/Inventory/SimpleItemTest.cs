using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    public class SimpleItemTest : MonoBehaviour
    {
        public Agent agent;
        public ItemDataSO itemData;

        [Header("Slots")]
        public int smallCount = 4;
        public SlotSizeSO smallSize;
        public int mediumCount = 1;
        public SlotSizeSO mediumSize;

        private void Start()
        {
            SlotInventory inventory = agent.inventory as SlotInventory;
            for (int i = 0; i < smallCount; i++)
            {
                inventory.AddSlot(smallSize);
            }
            for (int i = 0; i < mediumCount; i++)
            {
                inventory.AddSlot(mediumSize);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) { AddItem(); }
            if (Input.GetKeyDown(KeyCode.DownArrow)) { RemoveItem(); }
        }

        private void AddItem()
        {
            agent.inventory.TryAssignItem(itemData);
        }

        private void RemoveItem()
        {
            agent.inventory.RemoveItem(itemData);
        }
    }
}
