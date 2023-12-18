using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "13Mirror_Of_Reflection", menuName = "ScriptableObjects/Items/T2/13: Mirror of Reflection", order = 213)]
    public class Item13SO : ItemDataSO, ITakeDamageProcessor
    {
        public class MirrorReflectionItemVars : Item.ItemVars
        {
            public float totalDamage;
            public float reflectionDamageMultiplier;
        }

        [Header("Priority")]
        [SerializeField] private int priority;
        public int GetPriority() { return priority; }

        [Header("Damage")]
        [SerializeField] private float damageMultiplier;
        [SerializeField] private float bonusDamageMultiplier;

        //Initialize vars

        public override void InitializeVars(Item item)
        {
            item.vars = new MirrorReflectionItemVars { reflectionDamageMultiplier = 0};
        }

        //Manage stacks

        public override void AddStack(Item item)
        {
            MirrorReflectionItemVars vars = item.vars as MirrorReflectionItemVars;
            if (item.stacks == 1) { vars.reflectionDamageMultiplier += damageMultiplier; }
            else { vars.reflectionDamageMultiplier += bonusDamageMultiplier; }
        }

        public override void RemoveStack(Item item)
        {
            MirrorReflectionItemVars vars = item.vars as MirrorReflectionItemVars;
            if(item.stacks == 0) { vars.reflectionDamageMultiplier -= damageMultiplier; }
            else { vars.reflectionDamageMultiplier -= bonusDamageMultiplier; }
        }

        //Process HitEvents

        public void ProcessTakeDamage(ref HitEvent hitEvent, Item sourceItem)
        {
            if (!hitEvent.hasAgentSource) return;
            (sourceItem.vars as MirrorReflectionItemVars).totalDamage = hitEvent.GetTotalDamage();
            DamageSource(hitEvent, sourceItem);
        }
        public void ProcessTakeDamage(ref HitEvent hitEvent, List<StatusEffectHandler.EffectVars> vars) { }

        private void DamageSource(HitEvent hitEvent, Item item)
        {
            MirrorReflectionItemVars vars = item.vars as MirrorReflectionItemVars;

            HitEvent hitSource = new HitEvent();
            hitSource.baseDamage = vars.totalDamage * vars.reflectionDamageMultiplier;
            hitEvent.source.health.Hurt(hitSource);
        }

        public override string GenerateLongDescription()
        {
            return "When taking damage, reflect the damage, " +
                   $"dealing <color=#{HighlightColor}>{damageMultiplier * 100}%</color> " +
                   $"<color=#{StackColor}>(+{bonusDamageMultiplier * 100}% per stack)</color> damage of damage taken.";
        }
    }
}
