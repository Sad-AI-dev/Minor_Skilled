using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    [CreateAssetMenu(fileName = "Tier", menuName = "ScriptableObjects/Items/Item Tier", order = -500)]
    public class ItemTierSO : ScriptableObject
    {
        public string title;
        public Color titleColor;

        [Header("Image Settings")]
        public Sprite pickupBackground;

        [Header("Item visuals Settings")]
        public Material itemMat;
        public Color lightColor;
    }
}
