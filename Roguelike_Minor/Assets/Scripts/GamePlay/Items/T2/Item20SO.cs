using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.Data;
using System;

namespace Game {
    [CreateAssetMenu(fileName = "20Shattered_Soul", menuName = "ScriptableObjects/Items/T2/20: Shattered Soul", order = 220)]
    public class Item20SO : ItemDataSO, IDealDamageProcessor
    {
        private class Item20Vars : Item.ItemVars
        {
            public Item holder;
            public BehaviourPool<SoulFragmentProjectile> projectilePool;
            public float damageMult;
        }

        [Header("Priority")]
        public int priority;
        public int GetPriority() { return priority; }

        [Header("Damage Settings")]
        public float procChance;
        public float baseDamageMult;
        public float bonusDamageMult;
        public float vulnerableDamageMult;

        [Header("Projectile Settings")]
        public GameObject prefab;
        public float moveSpeed;
        public float procCoef = 1f;
        public float damageRange;
        public float effectRange;

        [Header("Status Effect Settings")]
        public StatusEffectSO vulnerableEffect;

        //========= Initialize Vars ===========
        public override void InitializeVars(Item item)
        {
            item.vars = new Item20Vars()
            {
                holder = item,
                projectilePool = new BehaviourPool<SoulFragmentProjectile>(prefab)
            };
        }

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            Item20Vars vars = item.vars as Item20Vars;
            if (item.stacks == 1) { vars.damageMult += baseDamageMult; }
            else { vars.damageMult += bonusDamageMult; }
        }

        public override void RemoveStack(Item item)
        {
            Item20Vars vars = item.vars as Item20Vars;
            if (item.stacks == 0) { vars.damageMult += baseDamageMult; }
            else { vars.damageMult += bonusDamageMult; }
        }

        //========== Process Deal Damage ==========
        public void ProcessDealDamage(ref HitEvent hitEvent, Item sourceItem)
        {
            ValueTuple<HitEvent, Item20Vars> data = new (hitEvent, sourceItem.vars as Item20Vars); //modern constructor
            //try shoot projectile
            AgentRandom.TryProc(procChance, sourceItem.agent, ShootProjectile, data);
        }
        public void ProcessDealDamage(ref HitEvent hitevent, List<StatusEffectHandler.EffectVars> vars) { }

        //========= Shoot Projectile ============
        private void ShootProjectile(ValueTuple<HitEvent, Item20Vars> data)
        {
            //cache data
            HitEvent hitEvent = data.Item1;
            Item20Vars vars = data.Item2;
            //create new projectile
            SoulFragmentProjectile proj = vars.projectilePool.GetBehaviour();
            SetupProjectile(proj, hitEvent, vars);
        }
        private void SetupProjectile(SoulFragmentProjectile proj, HitEvent hitEvent, Item20Vars vars)
        {
            //setup vars
            proj.settings = this;
            proj.owner = vars.holder;
            proj.sourceEvent = hitEvent;
            proj.damageMult = vars.damageMult;
            proj.targetPos = hitEvent.target.agent.transform.position + Vector3.up;
            //setup base position
            proj.transform.position = hitEvent.source.transform.position + Vector3.up;
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"Gain a <color=#{HighlightColor}>{procChance}% chance</color> to fire a " +
                $"<color=#{HighlightColor}>Soul Fragment</color>, dealing " +
                $"<color=#{HighlightColor}>{baseDamageMult * 100}%</color> " +
                $"<color=#{StackColor}>(+{bonusDamageMult * 100}% per stack)</color> " +
                $"damage in a {damageRange}m radius and applies " +
                $"<color=#{HighlightColor}>Vulnerable</color> in a {effectRange}m radius";
        }
    }
}
