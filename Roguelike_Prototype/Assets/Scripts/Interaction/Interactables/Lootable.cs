using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Lootable : MonoBehaviour
{
    public int price;
    public WeightedChance<GameObject> loot;

    [Header("UI Settings")]
    public TMP_Text priceLabel;

    private void Start()
    {
        priceLabel.text += price.ToString();
    }

    //====== Interact ======
    public void TryLoot()
    {
        if (GameManager.instance.CanAfford(price)) {
            GameManager.instance.PayMoney(price);
            DropLoot();
            Destroy(gameObject);
        }
    }

    private void DropLoot()
    {
        GameObject obj = Instantiate(loot.GetRandomEntry());
        obj.transform.position = transform.position + new Vector3(0, 1.5f, 0);
    }
}
