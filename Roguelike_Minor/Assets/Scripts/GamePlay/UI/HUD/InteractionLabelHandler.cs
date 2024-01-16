using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Game.Core;
using Game.Core.GameSystems;

namespace Game {
    public class InteractionLabelHandler : MonoBehaviour
    {
        [SerializeField] private UnityEvent onInteractFail;

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

        //animation vars
        private Animator animator;

        private void Awake()
        {
            EventBus<InteractFailEvent>.AddListener(OnInteractFail);
            animator = GetComponent<Animator>();
            //disable visuals
            defaultHolder.SetActive(false);
            itemLabelHolder.SetActive(false);
            gameObject.SetActive(false);
        }

        //========== Set Labels ===============
        public void InitializeLabel(Interactable interactable)
        {
            if (interactable.label != null && interactable.label != "")
            { //show default label
                //interactionLabel.text = "to " + interactable.label;
                interactionLabel.text = interactable.label;
                gameObject.SetActive(true);
                defaultHolder.SetActive(true);
                //hide item label
                itemLabelHolder.SetActive(false);
            }
            else if (interactable.TryGetComponent(out ItemPickup pickup))
            { //show item label
                SetupItemLabel(pickup.item);
                gameObject.SetActive(true);
                itemLabelHolder.SetActive(true);
                //hide default label
                defaultHolder.SetActive(false);
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

        //========= Handle Interact Fail ============
        private void OnInteractFail(InteractFailEvent eventData)
        {
            animator.Play("Error");
            onInteractFail?.Invoke();
        }

        //==== Handle Destroy ====
        private void OnDestroy()
        {
            EventBus<InteractFailEvent>.RemoveListener(OnInteractFail);
        }
    }
}
