using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour
{
    Inventory inv;
    public int ObjectID;

    public Image UIImage;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Value;
    public Button UIButton;

    public int Quantitiy;
    public TextMeshProUGUI QuantityUI;

    public Loot_SO item;

    private void Start()
    {
        UIButton = GetComponent<Button>();

        UIButton.onClick.AddListener(ButtonClick);
    }

    private void Update()
    {
        QuantityUI.text = Quantitiy.ToString();
    }

    void ButtonClick()
    {
        if (Quantitiy > 1) RemoveOne();
        else
        {
            inv.RemoveItem(this);
            Destroy(gameObject);
        }
    }

    public void ItemSet(Loot_SO newItem, Inventory pInv)
    {
        inv = pInv;
        item = newItem;
        UIImage.sprite = item.UISprite;
        Name.text = item.Name;
        Name.text = item.Value.ToString();
        ObjectID = newItem.ID;
        Quantitiy = 1;
    }

    public void AddOne()
    {
        Quantitiy++;
    }
    public void RemoveOne()
    {
        Quantitiy--;
    }

    public void RemoveAmount(int amount)
    {
        Quantitiy -= amount;
        if(Quantitiy <= 0)
        {
            inv.RemoveItem(this);
            Destroy(gameObject);
        }
    }
}
