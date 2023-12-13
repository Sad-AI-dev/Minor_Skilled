using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.GameSystems;

namespace Game {
    public class ItemGiver : MonoBehaviour
    {
        public ItemDataSO itemToGive;

        public void GiveItem(Interactor interactor)
        {
            interactor.agent.inventory.TryAssignItem(itemToGive);
        }
    }
}
