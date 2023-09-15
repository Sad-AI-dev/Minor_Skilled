using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedPickup : MonoBehaviour
{
    public FireRateItem item;

    private void Start()
    {
        GetComponent<ItemPickup>().item = item;
    }
}
