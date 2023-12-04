using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Core;
using Game.Core.GameSystems;

namespace Game {
    public class InteractionLabelHandler : MonoBehaviour
    {
        [Header("Default interaction label settings")]
        [SerializeField] private GameObject defaultHolder;
        [SerializeField] private TMP_Text interactionLabel;

        [Header("Item interaction label settings")]
        [SerializeField] private GameObject itemLabelHolder;
        [SerializeField] private Image itemBackground;
        [Space(10f)]
        [SerializeField] private Image itemIcon;
        [SerializeField] private TMP_Text itemTitle;
        [SerializeField] private TMP_Text itemDescription;
        [SerializeField] private TMP_Text itemTier;

        private void Awake()
        {
            //disable visuals
            defaultHolder.SetActive(false);
            itemLabelHolder.SetActive(false);
            gameObject.SetActive(false);
        }

        //========== Set Labels ===============
        public void InitializeLabel(Interactable interactable)
        {
            if (interactable.label != null && interactable.label != "")
            {
                interactionLabel.text = "to " + interactable.label;
                gameObject.SetActive(true);
                defaultHolder.SetActive(true);

            }
            else if (interactable.TryGetComponent(out ItemPickup pickup))
            {
                SetupItemLabel(pickup.item);
                gameObject.SetActive(true);
                itemLabelHolder.SetActive(true);
            }
        }

        private void SetupItemLabel(ItemDataSO itemData)
        {
            //setup background
            itemBackground.sprite = itemData.tier.pickupBackground;
            //icon
            itemIcon.sprite = itemData.UISprite;
            //==setup text elements==
            //title
            itemTitle.text = itemData.title;
            itemTitle.color = itemData.tier.titleColor;
            //description
            itemDescription.text = itemData.shortDescription;
            //tier
            itemTier.text = itemData.tier.title;
            itemTier.color = itemData.tier.titleColor;
        }

        //============ Handle Hide ==============
        private void OnDisable()
        {
            defaultHolder.SetActive(false);
            itemLabelHolder.SetActive(false);
        }
    }
}
