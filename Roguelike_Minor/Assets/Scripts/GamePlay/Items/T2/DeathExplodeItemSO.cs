using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "Death Explode Item", menuName = "ScriptableObjects/Items/T2/DeathExplode")]
    public class DeathExplodeItemSO : ItemDataSO
    {
        [Header("Damage Settings")]
        public float explosionDamageMult = 3f;
        //used when target gets hit by another death explode item effect 
        public float cascadeMult = 0.9f;

        [Header("Range Settings")]
        public float explosionRadius = 5f;
        public float bonusExplosionRadius = 2.5f;

        [Header("Timings")]
        public float explodeDelay = 0.1f;

        [Header("Technical Settings")]
        public GameObject explosionPrefab;
        
        //========= Manage Stacks ===========
        public override void AddStack(Item item) { }

        public override void RemoveStack(Item item) { }

        //========= Process Hit Events ===========
        public override void ProcessDealDamage(ref HitEvent hitEvent) 
        {
            hitEvent.onDeath.AddListener(Explode);
        }

        public override void ProcessTakeDamage(ref HitEvent hitEvent) { }

        //========= Process Heal Events ============
        public override void ProcessHealEvent(ref HealEvent healEvent) { }

        //========= Util Funcs ==========
        private float GetExplodeRadius(Item item)
        {
            return explosionRadius + ((item.stacks - 1) * bonusExplosionRadius);
        }

        private float CalcDamage(HitEvent hitEvent)
        {
            foreach (Item item in hitEvent.itemSources)
            {
                if (item.data == this)
                {
                    return hitEvent.baseDamage * cascadeMult;
                }
            }
            return hitEvent.GetTotalDamage() * explosionDamageMult;
        }

        //========= Spawn Explosion ===========
        private void Explode(HitEvent hitEvent) //instigating func
        {
            hitEvent.source.StartCoroutine(SpawnExplosionCo(hitEvent, hitEvent.target.transform.position));
        }
        
        private IEnumerator SpawnExplosionCo(HitEvent hitEvent, Vector3 pos)
        {
            yield return new WaitForSeconds(explodeDelay);
            SpawnExplosion(hitEvent, pos);
        }

        private void SpawnExplosion(HitEvent hitEvent, Vector3 pos) //create explosion in scene
        {
            Item sourceItem = hitEvent.source.inventory.GetItemOfType(this);

            //create range visuals
            GameObject obj = Instantiate(explosionPrefab);
            obj.transform.position = pos;
            obj.transform.localScale = new Vector3(2, 2, 2) * GetExplodeRadius(sourceItem);

            //sphere cast to deal damage
            Collider[] results = Physics.OverlapSphere(
                pos,
                GetExplodeRadius(sourceItem)
            );

            if (results.Length > 0)
            {
                for (int i = 0; i < results.Length; i++)
                {
                    if (results[i].CompareTag("Enemy"))
                    {
                        Agent enemy = results[i].gameObject.GetComponent<Agent>();
                        HitEvent hit = new HitEvent(hitEvent.source);
                        hit.baseDamage = CalcDamage(hitEvent);
                        hit.itemSources = new List<Item>(hitEvent.itemSources);
                        hit.itemSources.Add(sourceItem);
                        enemy.health.Hurt(hit);
                    }
                }
            }
        }
    }
}
