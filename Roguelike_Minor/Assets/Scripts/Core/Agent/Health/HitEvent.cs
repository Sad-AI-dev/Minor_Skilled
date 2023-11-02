using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Core {
    public class HitEvent
    {
        //source data
        public bool hasAgentSource;
        public Agent source;
        public List<Item> itemSources; //set if hitEvent is caused by item, prevents infinite procChaining

        //target
        public AgentHealthManager target;

        //proc coef
        //procCoef = Proc Coefficient, this is a multiplier applied to the likelyhood of a hitEvent proccing items.
        public float procCoef = 1f;

        //damage values
        public float baseDamage;
        public float damageMultiplier;
        public float damageReduction;
        //crit vars
        public bool isCrit;

        //death event
        public UnityEvent<HitEvent> onDeath;

        //ctor
        public HitEvent(Ability source = null)
        {
            hasAgentSource = source != null;

            if (hasAgentSource)
            {
                this.source = source.agent;
                procCoef = source.abilityData.procCoef;
                //setup base damage
                SetupBaseDamage(source);
            }

            InitializeVars();
        }
        //alternate ctor (for additional projectiles from items for example)
        public HitEvent(Agent source)
        {
            this.source = source;
            hasAgentSource = source != null;
            InitializeVars();
        }

        private void SetupBaseDamage(Ability source)
        {
            baseDamage = source.agent.stats.baseDamage * source.abilityData.damageMultiplier;
        }
        private void InitializeVars()
        {
            damageMultiplier = 1f;
            itemSources = new List<Item>();
            onDeath = new UnityEvent<HitEvent>();
            TryCrit();
        }
        private void TryCrit()
        {
            if (hasAgentSource)
            {
                //attempt to crit
                isCrit = AgentRandom.TryProc(source.stats.critChance, source);
            }
        }

        //============== Get Total Damage ===============
        public float GetTotalDamage()
        {
            return Mathf.Max(CalcDamage(), 1); //never allow take 0 damage
        }

        private float CalcDamage()
        {
            return (baseDamage * CalcTotalDamageMult()) - damageReduction;
        }
        //apply crit damage
        private float CalcTotalDamageMult()
        {
            float total = damageMultiplier;
            if (isCrit && hasAgentSource)
            {
                total += source.stats.critMult;
            }
            return total;
        }
    }
}
