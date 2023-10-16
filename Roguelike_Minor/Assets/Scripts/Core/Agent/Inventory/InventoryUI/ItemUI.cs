using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game.Core {
    public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("External Components")]
        public Image img;
        public Image bgImage;
        public CanvasGroup group;

        //vars
        [HideInInspector] public InventoryUI inventoryUI;
        public Item item { get; private set; }

        public void GenerateVisuals(Item item)
        {
            if (item != null) 
            { 
                this.item = item;
                img.color = Color.white;
                img.sprite = item.data.UISprite;
                group.blocksRaycasts = true;
            }
            ShowBGImage();
        }

        private void ShowBGImage()
        {
            bgImage.color = Color.white;
        }

        public void ResetVisuals()
        {
            item = null;
            img.color = new Color(1, 1, 1, 0);
            group.blocksRaycasts = false;
            bgImage.color = new Color(1, 1, 1, 0);
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
