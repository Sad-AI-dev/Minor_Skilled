using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Game.Core;
using Game.Core.GameSystems;
using TMPro;

namespace Game {
    public class Lootable : MonoBehaviour
    {
        [SerializeField] private LootTableSO lootTable;

        [Header("Technical")]
        [SerializeField] private GameObject itemTemplate;

        //luck var
        [HideInInspector] public float lootLuck = 0f;

        public void DropLoot()
        {
            GameObject obj = Instantiate(itemTemplate);
            //setup item data
            obj.GetComponent<ItemPickup>().item = lootTable.GetLoot(lootLuck);
            //set item pos
            obj.transform.SetPositionAndRotation(transform.position + transform.forward, GetRandomRotation());
        }

        private Quaternion GetRandomRotation()
        {
            return Quaternion.Euler(new Vector3(0, Random.Range(0, 360f), 0));
        }
    }
}
