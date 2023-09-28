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
            if (Input.GetKeyDown(KeyCode.W)) { AddSlot(); }
            if (Input.GetKeyDown(KeyCode.S)) { RemoveSlot(); }
        }

        private void AddItem()
        {
            agent.inventory.TryAssignItem(itemData);
        }

        private void RemoveItem()
        {
            agent.inventory.RemoveItem(itemData);
        }

        private void AddSlot()
        {
            (agent.inventory as SlotInventory).AddSlot(smallSize);
        }
        private void RemoveSlot()
        {
            SlotInventory inventory = agent.inventory as SlotInventory;
            if (inventory.slots.Count == 0) { return; }
            inventory.RemoveSlot(inventory.slots[0]); //remove last slot
        }
    }
}
