using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour
{
    Inventory inv;

    public Image UIImage;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Value;
    public Button UIButton;

    public Loot_SO item;

    private void Start()
    {
        UIButton = GetComponent<Button>();

        UIButton.onClick.AddListener(ButtonClick);
    }

    void ButtonClick()
    {
        inv.RemoveItem(gameObject);
        Destroy(gameObject);
    }

    public void ItemSet(Loot_SO newItem, Inventory pInv)
    {
        inv = pInv;
        item = newItem;
        UIImage.sprite = item.UISprite;
        Name.text = item.Name;
        Name.text = item.Value.ToString();
    }
}
