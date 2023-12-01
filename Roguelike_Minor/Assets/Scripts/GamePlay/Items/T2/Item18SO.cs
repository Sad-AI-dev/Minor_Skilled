using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "18Condensed_Planetarium", menuName = "ScriptableObjects/Items/T2/18: Condensed Planetarium", order = 218)]
    public class Item18SO : ItemDataSO
    {
        public class Item18Vars : Item.ItemVars
        {
            public Item holder;

            public List<PlanetProjectile> projectiles;
            //ring data
            public int currentRingIndex;
            public int ringCapacity;
            public int currentProjectilesInRing;

            public float ringRadius;

            //damage data
            public float damageMult;
        }

        [Header("Balance Settings")]
        public int basePlanets = 3;
        public int bonusPlanets = 3;
        [Space(10f)]
        public float baseDamageMult = 1.5f;
        public float bonusDamageMult = 0.5f;

        [Header("Prefab Settings")]
        public GameObject planetPrefab;

        [Header("Ring Settings")]
        public float ringRadius = 3f;
        public int ringCapacity = 3;
        [Space(10f)]
        public float bonusRingRadius = 2f;
        public int bonusRingCapacity = 2;

        //========= Initialize Vars ==========
        public override void InitializeVars(Item item)
        {
            item.vars = new Item18Vars {
                holder = item,
                ringCapacity = ringCapacity,
                ringRadius = ringRadius
            };
        }

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            Item18Vars vars = item.vars as Item18Vars;
            //update data
            UpdateDamageData(vars);
            UpdateRingVars(vars);
            SetupProjectile(vars);
        }

        //damage vars
        private void UpdateDamageData(Item18Vars vars)
        {
            if (vars.holder.stacks == 1) { vars.damageMult += baseDamageMult; }
            else { vars.damageMult += bonusDamageMult; }
        }

        //ring vars
        private void UpdateRingVars(Item18Vars vars)
        {
            vars.currentProjectilesInRing++;
            if (vars.currentProjectilesInRing >= vars.ringCapacity)
            {
                IncreaseRingIndex(vars);
            }
        }
        private void IncreaseRingIndex(Item18Vars vars)
        {
            vars.currentRingIndex++;
            vars.currentProjectilesInRing = 0;
            vars.ringCapacity = GetRingCapacity(vars.currentRingIndex);
            vars.ringRadius = GetRingRadius(vars.currentRingIndex);
        }
        private int GetRingCapacity(int ringIndex)
        {
            return ringCapacity + (ringIndex * bonusRingCapacity);
        }
        private float GetRingRadius(float ringIndex)
        {
            return ringRadius + (ringIndex * bonusRingRadius);
        }

        //setup projectile
        private void SetupProjectile(Item18Vars vars)
        {
            GameObject obj = Instantiate(planetPrefab, vars.holder.agent.transform);
            PlanetProjectile proj = obj.GetComponent<PlanetProjectile>();
            //register projectile
            vars.projectiles.Add(proj);
            //setup vars
            SetupProjectileVars(proj, vars);
        }
        private void SetupProjectileVars(PlanetProjectile proj, Item18Vars vars)
        {
            //TODO setup vars, look at planetProjectile class for what it needs
            //TODO everything in the planetProjectile class
            //TODO remove item logic
        }

        //========= Manage Remove Stack ==========
        public override void RemoveStack(Item item)
        {
            Item18Vars vars = item.vars as Item18Vars;
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"Temp Description.\n\n" +
                $"If you are seeing this, let Arvid know he's a dumbass";
        }
    }
}
