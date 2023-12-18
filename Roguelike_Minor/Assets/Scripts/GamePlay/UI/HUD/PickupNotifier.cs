using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Core;
using Game.Core.GameSystems;

namespace Game {
    public class PickupNotifier : MonoBehaviour
    {
        [Header("Timings")]
        [SerializeField] private float fadeInTime = 0.1f;
        [SerializeField] private float visibleTime = 1f;
        [SerializeField] private float fadeOutTime = 0.3f;
        [SerializeField] private float invisibleTime = 0.2f;

        [Header("Refs")]
        [SerializeField] private CanvasGroup group;
        [Space(10f)]
        [SerializeField] private TMP_Text nameLabel;
        [SerializeField] private TMP_Text descriptionLabel;
        [SerializeField] private Image itemIcon;
        [Header("Sound")]
        [SerializeField] private AudioPlayer audioPlayer;

        //vars
        private Queue<ItemDataSO> itemQueue;
        private float timer;

        private void Awake()
        {
            //initialize
            group.alpha = 0f;
            itemQueue = new Queue<ItemDataSO>();
            if (!audioPlayer) { audioPlayer = GetComponent<AudioPlayer>(); }
            //listen to events
            EventBus<PickupItemEvent>.AddListener(HandlePickupItem);
        }

        //======== Handle Pickup Item ==========
        private void HandlePickupItem(PickupItemEvent eventData)
        {
            //show visuals
            if (!itemQueue.Contains(eventData.item))
            { //only queue new unique items (prevents 10 duplicates playing in sequence)
                itemQueue.Enqueue(eventData.item);
                //added first item in queue check
                if (itemQueue.Count == 1)
                {
                    StartCoroutine(ShowNotifierCo());
                }
            }
            //play sound
            audioPlayer.Play();
        }

        //======= Visuals =======
        private IEnumerator ShowNotifierCo()
        {
            while (itemQueue.Count > 0)
            {
                //setup label
                SetupVisuals();
                //fade in
                yield return FadeInCo();
                //stay visable
                yield return new WaitForSeconds(visibleTime);
                //fade out
                yield return FadeOutCo();
                //delay next item
                yield return new WaitForSeconds(invisibleTime);
                //dequeue
                itemQueue.Dequeue();
            }
        }

        private void SetupVisuals()
        {
            ItemDataSO item = itemQueue.Peek();
            //setup UI elements
            nameLabel.text = item.title;
            descriptionLabel.text = item.shortDescription;
            itemIcon.sprite = item.UISprite;
        }

        //====== Fade Effects ========
        private IEnumerator FadeInCo()
        {
            timer = 0f;
            while (timer < fadeInTime) {
                timer += Time.deltaTime;
                group.alpha = timer / fadeInTime;
                yield return null; //loop
            }
        }

        private IEnumerator FadeOutCo()
        {
            timer = 0f;
            while (timer < fadeOutTime) {
                timer += Time.deltaTime;
                group.alpha = 1f - (timer / fadeOutTime);
                yield return null; //loop
            }
        }

        //======== Handle Destroy ==========
        private void OnDestroy()
        {
            EventBus<PickupItemEvent>.RemoveListener(HandlePickupItem);
        }
    }
}
