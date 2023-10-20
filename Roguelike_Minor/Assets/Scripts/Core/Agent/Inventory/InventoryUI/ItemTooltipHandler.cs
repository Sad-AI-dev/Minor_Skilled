using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game.Core {
    public class ItemTooltipHandler : MonoBehaviour
    {
        public InventoryUI inventoryUI;

        [Header("Position Settings")]
        [SerializeField] private Vector2 offset;

        [Header("Component Refs")]
        [SerializeField] private TMP_Text titleLabel;
        [SerializeField] private TMP_Text tierLabel;
        [SerializeField] private TMP_Text descriptionLabel;

        //vars
        private RectTransform rt;

        //===== Initialize =====
        private void Awake()
        {
            rt = GetComponent<RectTransform>();
            inventoryUI.onHoverItem += HandleHoverItem;
            gameObject.SetActive(false);
        }

        //====== Handle Position =====
        private void Update()
        {
            Move();
        }

        private void Move()
        {
            rt.anchoredPosition = (Vector2)Input.mousePosition + offset;
        }

        //====== Handle Hover Item ======
        private void HandleHoverItem(ItemUI itemUI)
        {
            if (itemUI) //item is not null
            {
                ActivateHover(itemUI);
            }
            else { gameObject.SetActive(false); }
        }

        private void ActivateHover(ItemUI itemUI)
        {
            gameObject.SetActive(true);
            //set data
            titleLabel.text = itemUI.item.data.title;
            tierLabel.text = itemUI.item.data.tier.title;
            descriptionLabel.text = itemUI.item.data.GenerateLongDescription();
        }

        //========= Unsub events ======
        private void OnDestroy()
        {
            inventoryUI.onHoverItem -= HandleHoverItem;
        }
    }
}
