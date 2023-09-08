using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item
{
    public string name;
    public string description;

    public GameObject prefab;
    public Sprite inventoryVisuals;

    [HideInInspector] public int stacks = 1;

    //============== Add Effect =============
    public abstract void AddNewEffect(Player player);
    public abstract void AddStack(Player player);

    //============== Remove Effect ================
    public abstract void RemoveStack(Player player);
    public abstract void RemoveEffect(Player player);
}
