using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Data;

namespace Game.Core {
    [RequireComponent(typeof(SlotInventory))]
    public class SlotInitializer : MonoBehaviour
    {
        [SerializeField] private UnityDictionary<SlotSizeSO, int> slotsToInitialize;

        private void Start()
        {
            SlotInventory inventory = GetComponent<SlotInventory>();

            foreach (var kvp in slotsToInitialize)
            {
                for (int i = 0; i < kvp.Value; i++)
                {
                    inventory.AddSlot(kvp.Key);
                }
            }
        }
    }
}
