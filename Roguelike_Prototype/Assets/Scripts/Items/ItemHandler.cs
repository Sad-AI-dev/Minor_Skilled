using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public Dictionary<string, Item> heldItems;

    //external components
    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
        heldItems = new Dictionary<string, Item>();
    }

    //================= Add Item ==================
    public void AddItem(Item item) {
        if (!heldItems.ContainsKey(item.name)) {
            heldItems.Add(item.name, item);
            item.AddNewEffect(player);
        }
        else {
            heldItems[item.name].stacks++;
            item.AddStack(player);
        }
    }

    //================ Remove Item =================
    public void RemoveItem(Item item)
    {
        if (heldItems.ContainsKey(item.name)) {
            if (heldItems[item.name].stacks == 1) {
                heldItems.Remove(item.name);
                item.RemoveEffect(player);
            }
            else {
                heldItems[item.name].stacks--;
                item.RemoveStack(player);
            }
        }
    }
}
