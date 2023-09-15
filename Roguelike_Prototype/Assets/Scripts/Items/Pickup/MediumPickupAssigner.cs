using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumPickupAssigner : MonoBehaviour
{
    //=========================
    // TEMP CLASS
    //=========================

    public MedTestItem item;

    private void Start()
    {
        GetComponent<ItemPickup>().item = item;
    }
}
