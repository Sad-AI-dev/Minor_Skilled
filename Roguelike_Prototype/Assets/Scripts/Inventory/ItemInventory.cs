using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    [Header("TestSettings")]
    public int smallSlots = 4;
    public int mediumSlots = 1;

    //[Header("Contents")]
    [HideInInspector] public List<ItemSlot> slots;
    [Header("External Components")]
    public Player player;

    private void Start()
    {
        player.inventory = this;

        for (int i = 0; i < smallSlots; i++) {
            AddSlot(ItemSlot.SlotSize.small);
        }
        for (int i = 0; i < mediumSlots; i++) {
            AddSlot(ItemSlot.SlotSize.medium);
        }
    }

    //=========== Manage Slots =============
    public void AddSlot(ItemSlot.SlotSize size)
    {
        slots.Add(new ItemSlot(size));
    }

    //========== Manage Items ==============
    public void AddItem(Item item)
    {
        for (int i = 0; i < slots.Count; i++) {
            if (slots[i].CanItemFit(item.size)) {
                slots[i].AssignItem(item);
                //add stack
                item.AddStack(player);
                break;
            }
        }
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < slots.Count; i++) {
            for (int j = 0; j < slots[i].contents.Count; j++) {
                if (slots[i].contents[j].name.Equals(item.name)) {
                    slots[i].UnassignItem(j);
                    //remove stack
                    item.RemoveStack(player);
                    return;
                }
            }
        }
    }

    public void DropItem(Item item)
    {
        RemoveItem(item);
        item.DropItem(player);
    }

    //============ Has Empty Slot Check ==============
    public bool HasSpace(ItemSlot.SlotSize size)
    {
        for(int i = 0; i < slots.Count; i++) {
            if (slots[i].CanItemFit(size)) { return true; }
        }
        return false;
    }
}
