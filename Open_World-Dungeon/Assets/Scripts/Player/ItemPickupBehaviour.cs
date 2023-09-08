using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupBehaviour : MonoBehaviour
{
    public Inventory Inventory;
    public Camera Cam;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            pickup();
        }
    }

    void pickup()
    {
        RaycastHit hit;
        if (Physics.Raycast(Cam.transform.position, Camera.main.transform.forward, out hit, 10f))
        {
            if (hit.transform.tag == "Item")
            {
                Inventory.PickupItem(hit.transform.GetComponent<ItemForPickup>().item);
                Destroy(hit.transform.gameObject);
            }
        }
    }
}
