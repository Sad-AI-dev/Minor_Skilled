using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [HideInInspector] public Item item;

    public void ObtainItem(Interactor player)
    {
        ItemInventory inventory = player.GetComponent<Player>().inventory;
        if (inventory.HasSpace(item.size)) {
            inventory.AddItem(item);
            Destroy(gameObject);
        }
    }
}
