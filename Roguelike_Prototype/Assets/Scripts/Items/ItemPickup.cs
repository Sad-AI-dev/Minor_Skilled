using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemPickup : MonoBehaviour
{
    public void ObtainItem(Interactor player)
    {
        player.GetComponent<ItemHandler>().AddItem(GetItem());
        Destroy(gameObject);
    }

    //Define item
    public abstract Item GetItem();
}
