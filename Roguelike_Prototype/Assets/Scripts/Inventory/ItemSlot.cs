using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSlot
{
    [System.Serializable]
    public enum SlotSize { small = 1, medium = 4 };
    public enum SlotState { open, filled };

    public SlotSize size;
    public SlotState State { get { return budget > 0 ? SlotState.open : SlotState.filled; } }
    //holds contents of a filled slot
    public List<Item> contents;
    //amount of slots open || 1 = 1x1 slot
    private int budget;

    //ctor
    public ItemSlot(SlotSize size)
    {
        this.size = size;
        contents = new List<Item>();
        budget = DetermineBudget();
    }
    private int DetermineBudget()
    {
        return size switch {
            SlotSize.medium => 4,
            _ => 1,
        };
    }

    //========== Assign Item ============
    public bool CanItemFit(SlotSize size)
    {
        return budget >= (int)size;
    }

    public void AssignItem(Item item)
    {
        budget -= (int)item.size;
        contents.Add(item);
    }

    //========== Unassign Item ============
    public void UnassignItem(int index) {
        budget += (int)contents[index].size;
        contents.RemoveAt(index);
    }
}
