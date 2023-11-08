using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    public class GameScalingManager : MonoBehaviour
    {
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }
        public static GameScalingManager instance;

        [Header("Enemy Scaling Settings")]
        [SerializeField] private float enemyScalingFrequency = 10f; //expressed in seconds

        [Header("Price Scaling Settings")]
        [SerializeField] private float priceScalingFrequency = 20f; //expressed in seconds
        [SerializeField] private float priceScalingAmount = 0.1f;

        //=== game scaling vars ===
        [HideInInspector] public int enemyLevel = 0;
        [HideInInspector] public float priceMult = 1f;

        private void Start()
        {
            StartCoroutine(ScaleEnemyLevelCo());
            StartCoroutine(ScalePriceCo());
        }

        //=================== Enemy Scaling =====================
        private IEnumerator ScaleEnemyLevelCo()
        {
            yield return new WaitForSeconds(enemyScalingFrequency);
            //activate enemy level up event
            enemyLevel++;
            EventBus<EnemyLevelupEvent>.Invoke(new EnemyLevelupEvent() { level = enemyLevel });
            //loop
            StartCoroutine(ScaleEnemyLevelCo());
        }

        //=================== Price Scaling ======================
        private IEnumerator ScalePriceCo()
        {
            yield return new WaitForSeconds(priceScalingFrequency);
        }
    }
}
