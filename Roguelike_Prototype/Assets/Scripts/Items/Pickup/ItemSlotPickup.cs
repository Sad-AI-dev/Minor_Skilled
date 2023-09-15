using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlotPickup : MonoBehaviour
{
    public ItemSlot.SlotSize size;

    public void AddItemSlot()
    {
        GameManager.instance.player.inventory.AddSlot(size);
        Destroy(gameObject);
    }
}
