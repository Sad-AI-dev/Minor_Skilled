using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    [CreateAssetMenu(fileName = "size", menuName = "ScriptableObjects/Agent/SlotSize")]
    public class SlotSizeSO : ScriptableObject
    {
        public int capacity = 1;
    }
}
