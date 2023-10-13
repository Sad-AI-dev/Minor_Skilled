using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    public class InventoryTest : MonoBehaviour
    {
        public SlotInventory testInventory;
        public ItemDataSO testItem;
        public SlotSizeSO testSlotSize;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) { AddItem(); }
            if (Input.GetKeyDown(KeyCode.DownArrow)) { RemoveItem(); }
            if (Input.GetKeyDown(KeyCode.W)) { AddSlot(); }
            if (Input.GetKeyDown(KeyCode.S)) { RemoveSlot(); }
        }

        //Item tests
        private void AddItem()
        {
            Debug.Log(testInventory.TryAssignItem(testItem));
        }

        private void RemoveItem()
        {
            testInventory.RemoveItem(testItem);
        }

        //slot tests
        private void AddSlot()
        {
            testInventory.AddSlot(testSlotSize);
        }

        private void RemoveSlot()
        {
            testInventory.RemoveSlot(testSlotSize);
        }
    }
}
