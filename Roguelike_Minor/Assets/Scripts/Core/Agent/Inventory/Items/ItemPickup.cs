using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Core.GameSystems;

namespace Game.Core {
    public class ItemPickup : MonoBehaviour
    {
        public ItemDataSO item;

        [Header("Visual Settings")]
        [SerializeField] private Image spriteHolder;
        [SerializeField] private MeshRenderer matHolder;
        [SerializeField] private Light pointLight;

        //========= Generate Visuals ========
        private void Start()
        {
            GenerateVisuals();
        }

        public void GenerateVisuals()
        {
            item.GenerateVisuals(spriteHolder, matHolder, pointLight);
        }

        //========= Handle Pickup =========
        public void OnPickup(Interactor interactor)
        {
            if (interactor.agent)
            {
                if (interactor.agent.inventory.TryAssignItem(item))
                {
                    //notify item was picked up
                    EventBus<PickupItemEvent>.Invoke(new PickupItemEvent() { item = item });
                    //succesfully picked up item
                    Destroy(gameObject);
                }
            }
        }
    }
}
