using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<Loot_SO, GameObject> items = new Dictionary<Loot_SO, GameObject>();
    public GameObject Background;
    public Transform vertLayoutGroup;
    public GameObject InvItemPrefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !Game.instance.InShop)
        {
            if (Cursor.lockState == CursorLockMode.Locked) Cursor.lockState = CursorLockMode.None;
            else if (Cursor.lockState == CursorLockMode.None) Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = !Cursor.visible;
            Background.SetActive(!Background.activeSelf);
            Pause();
        }
    }

    private void Pause()
    {
        Game.instance.Pause = !Game.instance.Pause;
        if (Time.timeScale != 0) Time.timeScale = 0;
        else { Time.timeScale = 1; }
    }

    public void PickupItem(Loot_SO loot)
    {
        Debug.Log("Contains key: " + items.ContainsKey(loot));
        if (items.ContainsKey(loot))
        {
            items[loot].GetComponent<InventoryItem>().AddOne();
        }
        else
        {
            GameObject newItem = Instantiate(InvItemPrefab, vertLayoutGroup);
            newItem.GetComponent<InventoryItem>().ItemSet(loot, this);
            items.Add(loot, newItem);
        }
    }

    public void RemoveItem(InventoryItem item)
    {
        items.Remove(item.item);
    }

    public void RemoveAmount(UpgradeCost UpgradeCost)
    {
        foreach (var item in UpgradeCost.cost)
        {
            InventoryItem invItem = items[item.loot].GetComponent<InventoryItem>();
            invItem.RemoveAmount(item.amount);
        }

    }
}
