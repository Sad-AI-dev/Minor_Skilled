using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core {
    public class ItemUI : MonoBehaviour
    {
        [HideInInspector] public Inventory inventory;
        [HideInInspector] public Item item;

        [Header("External Components")]
        public Image img;

        public void GenerateVisuals(Item item)
        {
            this.item = item;
            img.sprite = item.data.UISprite;
        }
    }
}
