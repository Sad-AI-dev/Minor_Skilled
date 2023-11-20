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
        public float procCoef;

        //damage values
        public float baseDamage;
        public float damageMultiplier;
        public float damageReduction;
        //crit vars
        public bool isCrit;

        //death event
        public UnityEvent<HitEvent> onDeath;

        //UI
        public Color labelColor = Color.white;
        public Color critColor;

        //============== Constructors =============
        //=== Default Constructor ===
        public HitEvent(Agent source = null, float procCoef = 1f)
        {
            this.source = source;
            this.procCoef = procCoef;
            //Setup vars
            hasAgentSource = source != null;
            if (hasAgentSource) { TryCrit(); }
            InitializeVars();
        }

        //=== Constructor for abilities ===
        public HitEvent(Ability source)
        {
            hasAgentSource = source != null;

            if (hasAgentSource)
            {
                this.source = source.agent;
                procCoef = source.abilityData.procCoef;
                //setup base damage
                SetupBaseDamage(source);
                TryCrit();
            }

            InitializeVars();
        }
        private void SetupBaseDamage(Ability source)
        {
            baseDamage = source.agent.stats.baseDamage * source.abilityData.damageMultiplier;
        }

        //=== Constructor for proc chain ===
        public HitEvent(HitEvent baseEvent, Item procItem)
        {
            source = baseEvent.source;
            hasAgentSource = true; //proc chain cannot happen without agent source
            //copy vars
            baseDamage = baseEvent.GetTotalDamage();
            procCoef = baseEvent.procCoef;
            isCrit = baseEvent.isCrit;
            //manage item
            itemSources = new List<Item>(baseEvent.itemSources);
            if (!itemSources.Contains(procItem)) { itemSources.Add(procItem); }
            //initialize
            InitializeVars();
        }

        //================= Initialize Vars ==================
        private void InitializeVars()
        {
            damageMultiplier = 1f;
            itemSources ??= new List<Item>();
            onDeath = new UnityEvent<HitEvent>();
            //setup colors
            labelColor = Color.white;
            critColor = new Color(0.9056604f, 0.5441936f, 0.07176924f, 1f);
        }
        
        private void TryCrit()
        {
            isCrit = false; //default value
            AgentRandom.TryProc(source.stats.critChance, this, () => isCrit = true);
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

        private float CalcTotalDamageMult()
        {
            float total = damageMultiplier;
            if (isCrit && hasAgentSource)
            { //apply crit damage
                total *= source.stats.critMult;
            }
            return total;
        }
    }
}
