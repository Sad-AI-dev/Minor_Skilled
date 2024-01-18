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
        [Space(10f)]
        [SerializeField] private LevitateOrb rerollOrb;
        [SerializeField] private float orbForce = 200f;

        [Header("Balance Settings")]
        [SerializeField] private int defaultReloads = 1;

        [Header("SlotPiece Settings")]
        [SerializeField] private float slotPieceChance = 10f;
        [SerializeField] private ItemDataSO slotPiece;

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
            //setup default vars
            rerolls = defaultReloads;
            //invoke events
            EventBus<ShopLoadedEvent>.Invoke(new ShopLoadedEvent() { shop = this });
            //generate visuals
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
            purchasable.Setup(GetItem());
            purchasable.transform.position = spawnPoints[index].position;
            //store ref
            purchasables[index] = purchasable;
        }

        private ItemDataSO GetItem()
        {
            if (Random.Range(0f, 100f) < slotPieceChance)
            {
                return slotPiece;
            }
            else { return lootTable.GetLoot(itemLuck); }
        }

        //================ Refresh ===========
        public void Refresh()
        {
            if (!isGenerating && rerolls > 0)
            {
                StartCoroutine(GenerateShopContentCo());
                if (rerollOrb) { rerollOrb.AddRandomForce(orbForce); }
                rerolls--;
            }
            else { EventBus<InteractFailEvent>.Invoke(new InteractFailEvent() { failCause = InteractFailCause.noRerrols }); }
        }

        //================= Reset Shop ====================
        private IEnumerator ResetShopCo()
        {
            if (purchasables != null)
            {
                for (int i = 0; i < purchasables.Length; i++)
                {
                    if (purchasables[i].gameObject.activeSelf) {
                        yield return new WaitForSeconds(spawnDelay);
                    }
                    //reset puchasable
                    purchasables[i].gameObject.SetActive(false);
                }
            }
            //initialize
            else { purchasables = new ShopPurchasable[spawnPoints.Length]; }
        }
    }
}
