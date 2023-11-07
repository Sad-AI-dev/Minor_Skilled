using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.Data;

namespace Game {
    public class Shop : MonoBehaviour
    {
        [Header("Visual Settings")]
        public Transform[] spawnPoints;
        [SerializeField] private float spawnDelay = 0.5f;

        [Header("Tech Settings")]
        [SerializeField] private LootTableSO lootTable;
        [SerializeField] private BehaviourPool<ShopPurchasable> pool;

        //vars
        private ShopPurchasable[] purchasables;
        private bool isGenerating;

        //item vars
        [HideInInspector] public float itemLuck;
        [HideInInspector] public int rerolls = 0;

        private void Awake()
        {
            EventBus<ShopLoadedEvent>.Invoke(new ShopLoadedEvent() { shop = this });
        }

        private void Start()
        {
            StartCoroutine(GenerateShopContentCo());
        }

        //===================== Generate Shop Contents =================
        private IEnumerator GenerateShopContentCo()
        {
            //initialize vars
            isGenerating = true;
            yield return ResetShopCo();
            //generate contents
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                yield return new WaitForSeconds(spawnDelay);
                GenerateShopSlot(i);
            }
            isGenerating = false;
        }

        private void GenerateShopSlot(int index)
        {
            ShopPurchasable purchasable = pool.GetBehaviour();
            //initialize purchasable
            purchasable.Setup(lootTable.GetLoot(itemLuck));
            purchasable.transform.position = spawnPoints[index].position;
            //store ref
            purchasables[index] = purchasable;
        }

        //================ Refresh ===========
        public void Refresh()
        {
            if (!isGenerating && rerolls > 0)
            {
                StartCoroutine(GenerateShopContentCo());
                rerolls--;
            }
        }

        //================= Reset Shop ====================
        private IEnumerator ResetShopCo()
        {
            if (purchasables != null)
            {
                for (int i = 0; i < purchasables.Length; i++)
                {
                    yield return new WaitForSeconds(spawnDelay);
                    //reset puchasable
                    purchasables[i].gameObject.SetActive(false);
                }
            }
            //initialize
            else { purchasables = new ShopPurchasable[spawnPoints.Length]; }
        }
    }
}
