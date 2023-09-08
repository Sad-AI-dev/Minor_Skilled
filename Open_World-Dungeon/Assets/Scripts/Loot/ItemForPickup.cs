using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemForPickup : MonoBehaviour
{
    public int ID;
    public Loot_SO item;

    private void Start()
    {
        ID = item.ID;
    }
}
