using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.Data;

namespace Game {
    [CreateAssetMenu(fileName = "lootTable", menuName = "ScriptableObjects/Loot Table")]
    public class LootTableSO : ScriptableObject
    {
        [System.Serializable]
        public struct Category
        {
            public string name;
            public List<ItemDataSO> options;
        }

        public WeightedChance<Category> categories;

        public ItemDataSO GetLoot()
        {
            return GetLoot(0f);
        }

        public ItemDataSO GetLoot(float luck)
        {
            Category chosenCategory = categories.GetRandomEntry(luck);
            return chosenCategory.options[Random.Range(0, chosenCategory.options.Count)];
        }
    }
}
