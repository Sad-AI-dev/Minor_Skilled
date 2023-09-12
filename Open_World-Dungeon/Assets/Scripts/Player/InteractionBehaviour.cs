using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBehaviour : MonoBehaviour
{
    public Inventory Inventory;
    public Camera Cam;

    public GameObject ShopUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !Game.instance.InInventory)
        {
            Interaction();
        }
        if (Game.instance.InShop && Input.GetKeyDown(KeyCode.Y))
        {
            ExitShop();
        }
    }

    public void ExitShop()
    {
        ShopUI.SetActive(false);
        Game.instance.Pause = false;
        Game.instance.InShop = false;
    }

    void Interaction()
    {
        RaycastHit hit;
        if (Physics.Raycast(Cam.transform.position, Camera.main.transform.forward, out hit, 10f))
        {
            if (hit.transform.tag == "Item")
            {
                Inventory.PickupItem(hit.transform.GetComponent<ItemForPickup>().item);
                Destroy(hit.transform.gameObject);
            }
            if(hit.transform.tag == "Shop")
            {
                ShopUI.SetActive(true);
                Game.instance.Pause = true;
                Game.instance.InShop = true;
            }
        }
    }
}
