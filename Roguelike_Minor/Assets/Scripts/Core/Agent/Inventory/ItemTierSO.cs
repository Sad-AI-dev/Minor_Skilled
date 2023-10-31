using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    [CreateAssetMenu(fileName = "Tier", menuName = "ScriptableObjects/Items/Item Tier", order = 40)]
    public class ItemTierSO : ScriptableObject
    {
        public string title;
        public Color titleColor;

        [Header("Image Settings")]
        public Sprite pickupBackground;
    }
}
