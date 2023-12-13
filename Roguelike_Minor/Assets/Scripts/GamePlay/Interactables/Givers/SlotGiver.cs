using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.GameSystems;

namespace Game {
    public class SlotGiver : MonoBehaviour
    {
        [SerializeField] private SlotSizeSO size;

        public void GiveSlot(Interactor interactor)
        {
            interactor.GetComponent<SlotInventory>().AddSlot(size);
        }
    }
}
