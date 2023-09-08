using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<GameObject> items = new List<GameObject>();
    public GameObject Background;
    public Transform vertLayoutGroup;
    public GameObject InvItemPrefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Background.SetActive(!Background.activeSelf);
        }
    }

    public void PickupItem(Loot_SO loot)
    {
        GameObject newItem = Instantiate(InvItemPrefab, vertLayoutGroup);
        newItem.GetComponent<InventoryItem>().ItemSet(loot, this);
        items.Add(newItem);
    }

    public void RemoveItem(GameObject item)
    {
        items.Remove(item);
    }
}
