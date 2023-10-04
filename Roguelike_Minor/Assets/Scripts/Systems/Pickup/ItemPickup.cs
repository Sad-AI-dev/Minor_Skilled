using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Systems {
    public class ItemPickup : MonoBehaviour
    {
        [SerializeField] private ItemDataSO item;

        public void OnPickup(Interactor interactor)
        {
            if (interactor.agent)
            {
                if (interactor.agent.inventory.TryAssignItem(item))
                { //succesfully picked up item
                    Destroy(gameObject);
                }
            }
        }
    }
}
