using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item
{
    public string name;
    public string description;
    public ItemSlot.SlotSize size;

    [Space(10f)]
    public GameObject pickupPrefab;
    public Sprite inventoryVisuals;

    //============== Add Effect =============
    public abstract void AddStack(Player player);

    //============== Remove Effect ================
    public abstract void RemoveStack(Player player);

    //============= Drop Item ==============
    public void DropItem(Player player)
    {
        Transform t = Object.Instantiate(pickupPrefab).transform;
        t.position = player.transform.position;
        t.GetComponent<ItemPickup>().item = this;
    }
}
