using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAssigner : MonoBehaviour
{
    //=========================
    // TEMP CLASS
    //=========================

    public TestItem item;

    private void Start()
    {
        GetComponent<ItemPickup>().item = item;
    }
}
