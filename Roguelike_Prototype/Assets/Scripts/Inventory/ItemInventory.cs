using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    [Header("TestSettings")]
    public int smallSlots = 4;
    public int mediumSlots = 1;

    [HideInInspector] public List<ItemSlot> slots;

    private Player player;
    private InventoryUI ui;

    [HideInInspector] public GameObject inventoryUI;

    private void Start()
    {
        player = GameManager.instance.player;
        player.inventory = this;
        ui = GetComponent<InventoryUI>();
        ui.inventory = this;
        inventoryUI = transform.GetChild(0).gameObject;

        slots = new List<ItemSlot>();
        for (int i = 0; i < smallSlots; i++) {
            AddSlot(ItemSlot.SlotSize.small);
        }
        for (int i = 0; i < mediumSlots; i++) {
            AddSlot(ItemSlot.SlotSize.medium);
        }

        //generate UI
        ui.GenerateVisuals();
        //dont destroy on load
        DontDestroyRegister.instance.RegisterObject(transform.parent.parent.gameObject);
    }

    //=========== Manage Slots =============
    public void AddSlot(ItemSlot.SlotSize size)
    {
        slots.Add(new ItemSlot(size));
        ui.GenerateVisuals(); // update UI
    }

    //========== Manage Items ==============
    public void AddItem(Item item)
    {
        if (!HasSpace(item.size)) { return; } //only assign when space available
        for (int i = 0; i < slots.Count; i++) {
            if (slots[i].CanItemFit(item.size)) {
                slots[i].AssignItem(item);
                //add stack
                item.AddStack(player);
                //update visuals
                ui.GenerateVisuals();
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
                    //update UI
                    ui.GenerateVisuals();
                    return;
                }
            }
        }
    }

    public void DropItem(Item item)
    {
        RemoveItem(item);
        item.DropItem(player);
        ReOrderInventory();
    }

    private void ReOrderInventory()
    {
        List<Item> items = new List<Item>();
        for (int i = 0; i < slots.Count; i++) {
            for (int j = 0; j < slots[i].contents.Count; j++) {
                Item item = slots[i].contents[j];
                items.Add(item);
                RemoveItem(item);
            }
        }
        //add items back to inventory
        for (int i = 0; i < items.Count; i++) {
            AddItem(items[i]);
        }
    }

    //============ Has Empty Slot Check ==============
    public bool HasSpace(ItemSlot.SlotSize size)
    {
        for(int i = 0; i < slots.Count; i++) {
            if (slots[i].CanItemFit(size)) { return true; }
        }
        return false;
    }

    //===== DEBUG =======
    public void DebugSlotState()
    {
        string msg = "==Inventory Contents==\n\n";
        for (int i = 0; i < slots.Count; i++) {
            msg += $"Slot {i}: size {slots[i].size}\nContents:\n";
            for (int j = 0; j < slots[i].contents.Count; j++) {
                msg += $" Item {j}:\n  Size: {slots[i].contents[j].size} || Name: {slots[i].contents[j].name}\n";
            }
            msg += "\n";
        }
        msg += "==========================";
        Debug.Log(msg);
    }
}
