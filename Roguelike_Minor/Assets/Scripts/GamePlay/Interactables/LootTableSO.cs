using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Data;

namespace Game {
    [CreateAssetMenu(fileName = "lootTable", menuName = "ScriptableObjects/Loot Table")]
    public class LootTableSO : ScriptableObject
    {
        [System.Serializable]
        public struct Category
        {
            public string name;
            public List<GameObject> options;
        }

        public WeightedChance<Category> categories;

        public GameObject GetLoot()
        {
            Category chosenCategory = categories.GetRandomEntry();
            return chosenCategory.options[Random.Range(0, chosenCategory.options.Count)];
        }
    }
}
