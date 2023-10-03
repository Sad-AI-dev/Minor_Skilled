using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game.Core {
    public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [HideInInspector] public InventoryUI inventoryUI;
        [HideInInspector] public Item item;

        [Header("External Components")]
        public Image img;

        public void GenerateVisuals(Item item)
        {
            this.item = item;
            img.color = Color.white;
            img.sprite = item.data.UISprite;
        }

        //=========== Pointer Event Handling =================
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (item != null)
            {
                inventoryUI.SetHoveredItem(this);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (item != null)
            {
                inventoryUI.SetHoveredItem(null); //reset hovered item
            }
        }
    }
}
