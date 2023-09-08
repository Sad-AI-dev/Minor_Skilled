using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItemPickup : ItemPickup
{
    public TestItem item;

    public override Item GetItem()
    {
        return item;
    }
}
