using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class Lootable : MonoBehaviour
    {
        [SerializeField] private LootTableSO lootTable;

        public void Loot()
        {
            GameObject obj = Instantiate(lootTable.GetLoot());
            obj.transform.SetPositionAndRotation(transform.position + transform.forward, GetRandomRotation());
            Destroy(gameObject);
        }

        private Quaternion GetRandomRotation()
        {
            return Quaternion.Euler(new Vector3(0, Random.Range(0, 360f), 0));
        }
    }
}
