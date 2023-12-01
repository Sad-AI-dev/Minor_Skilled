using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.GameSystems;

namespace Game {
    [CreateAssetMenu(fileName = "3Explosive_Greens", menuName = "ScriptableObjects/Items/T2/3: Explosive Greens", order = 203)]
    public class Item3SO : ItemDataSO
    {
        private class Item3Vars : Item.ItemVars
        {
            public float explodeRadius;
        }

        [Header("Proc Chance Settings")]
        public float procChance = 50f;

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
            item.vars = new Item3Vars();
        }

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            Item3Vars vars = item.vars as Item3Vars;
            if (item.stacks == 1) { vars.explodeRadius += explosionRadius; }
            else { vars.explodeRadius += bonusExplosionRadius; }
        }

        public override void RemoveStack(Item item)
        {
            Item3Vars vars = item.vars as Item3Vars;
            if (item.stacks == 0) { vars.explodeRadius -= explosionRadius; }
            else { vars.explodeRadius -= bonusExplosionRadius; }
        }

        //========= Process Hit Events ===========
        public override void ProcessDealDamage(ref HitEvent hitEvent, Item sourceItem) 
        {
            hitEvent.onDeath.AddListener(TryExplode);
        }

        //========= Util Funcs ==========
        private float GetExplodeRadius(Item item)
        {
            return (item.vars as Item3Vars).explodeRadius;
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
        private void TryExplode(HitEvent hitEvent)
        {
            AgentRandom.TryProc(procChance, hitEvent, Explode, hitEvent);
        }

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

            List<Agent> targets = Explosion.FindAgentsInRange(pos, explodeRadius, hitEvent.source);
            //deal damage
            foreach (Agent target in targets)
            {
                HitEvent hit = new HitEvent(hitEvent, sourceItem);
                //setup damage
                hit.baseDamage = CalcDamage(hitEvent);
                //deal damage
                target.health.Hurt(hit);
            }
        }

        //============== Description ===========
        public override string GenerateLongDescription()
        {
            return $"enemies have a " +
                $"<color=#{HighlightColor}>{procChance}%</color> chance to " +
                $"<color=#{HighlightColor}>explode</color> on death, dealing " +
                $"<color=#{HighlightColor}>{explosionDamageMult * 100}% TOTAL damage</color>\n" +
                $"in a <color=#{HighlightColor}>{explosionRadius}m radius</color> " +
                $"<color=#{StackColor}>(+{bonusExplosionRadius}m per stack)</color>";
        }
    }
}
