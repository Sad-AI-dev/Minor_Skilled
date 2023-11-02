using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "Death Explode Item", menuName = "ScriptableObjects/Items/T2/DeathExplode", order = 20)]
    public class DeathExplodeItemSO : ItemDataSO
    {
        public class DeathExplodeItemVars : Item.ItemVars
        {
            public float explodeRadius;
        }

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
        
        //========= Initialize Vars ============
        public override void InitializeVars(Item item)
        {
            item.vars = new DeathExplodeItemVars { explodeRadius = explosionRadius };
        }

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            (item.vars as DeathExplodeItemVars).explodeRadius += bonusExplosionRadius;
        }

        public override void RemoveStack(Item item)
        {
            DeathExplodeItemVars vars = item.vars as DeathExplodeItemVars;
            if (item.stacks == 0) { vars.explodeRadius -= explosionRadius; }
            else { vars.explodeRadius -= bonusExplosionRadius; }
        }

        //========= Process Hit Events ===========
        public override void ProcessDealDamage(ref HitEvent hitEvent, Item sourceItem) 
        {
            hitEvent.onDeath.AddListener(Explode);
        }

        public override void ProcessTakeDamage(ref HitEvent hitEvent, Item sourceItem) { }

        //========= Process Heal Events ============
        public override void ProcessHealEvent(ref HealEvent healEvent, Item sourceItem) { }

        //========= Util Funcs ==========
        private float GetExplodeRadius(Item item)
        {
            return (item.vars as DeathExplodeItemVars).explodeRadius;
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
            //cache data
            Item sourceItem = hitEvent.source.inventory.GetItemOfType(this);
            float explodeRadius = GetExplodeRadius(sourceItem);

            //create range visuals
            GameObject obj = Instantiate(explosionPrefab);
            obj.transform.position = pos;
            obj.transform.localScale = new Vector3(2, 2, 2) * explodeRadius;

            //sphere cast to deal damage
            Collider[] results = Physics.OverlapSphere(pos, explodeRadius);

            if (results.Length > 0)
            {
                for (int i = 0; i < results.Length; i++)
                {
                    if (results[i].CompareTag("Enemy"))
                    {
                        Agent enemy = results[i].gameObject.GetComponent<Agent>();
                        //create new hitevent
                        HitEvent hit = new HitEvent(hitEvent.source);
                        //setup damage
                        hit.baseDamage = CalcDamage(hitEvent);
                        //manage item sources
                        hit.itemSources = new List<Item>(hitEvent.itemSources);
                        hit.itemSources.Add(sourceItem);
                        //deal damage
                        enemy.health.Hurt(hit);
                    }
                }
            }
        }

        //============== Description ===========
        public override string GenerateLongDescription()
        {
            return $"enemies explode on death, dealing " +
                $"<color=#{HighlightColor}>{explosionDamageMult * 100}% TOTAL damage</color>\n" +
                $"in a <color=#{HighlightColor}>{explosionRadius}m radius</color> " +
                $"<color=#{StackColor}>(+{bonusExplosionRadius}m per stack)</color>";
        }
    }
}
