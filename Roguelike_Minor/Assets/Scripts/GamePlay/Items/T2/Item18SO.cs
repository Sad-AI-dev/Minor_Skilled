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

            public GameObject vfx;
            public List<PlanetProjectile> projectiles;
            //ring data
            public int currentRingIndex;
            public int ringCapacity;
            public int currentProjectilesInRing;

            public float ringRadius;

            //damage data
            public float procCoef;
            public float damageMult;
        }

        [Header("Balance Settings")]
        public int basePlanets = 3;
        public int bonusPlanets = 3;
        [Space(10f)]
        public float baseDamageMult = 1.5f;
        public float bonusDamageMult = 0f;
        public float procCoef = 1f;

        [Header("Prefab Settings")]
        public GameObject planetPrefab;
        public GameObject vfxPrefab;

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
                projectiles = new List<PlanetProjectile>(),
                ringCapacity = ringCapacity,
                ringRadius = ringRadius,
                procCoef = procCoef
            };
        }

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            Item18Vars vars = item.vars as Item18Vars;
            //update data
            int planetsToCreate = item.stacks == 1 ? basePlanets : bonusPlanets;
            for (int i = 0; i < planetsToCreate; i++)
            {
                UpdateDamageData(vars);
                UpdateRingVars(vars);
                SetupProjectile(vars);
            }
            //vfx
            if (item.stacks == 1) { vars.vfx = Instantiate(vfxPrefab, vars.holder.agent.transform); }
        }

        //damage vars
        private void UpdateDamageData(Item18Vars vars)
        {
            float damage = 0;
            if (vars.holder.stacks > 0) 
            {
                damage += baseDamageMult + bonusDamageMult * (vars.holder.stacks - 1);
            }
            vars.damageMult = damage;
        }

        //ring vars
        private void UpdateRingVars(Item18Vars vars)
        {
            vars.currentProjectilesInRing++;
            if (vars.currentProjectilesInRing > vars.ringCapacity)
            {
                IncreaseRingIndex(vars);
            }
        }
        private void IncreaseRingIndex(Item18Vars vars)
        {
            vars.currentRingIndex++;
            vars.currentProjectilesInRing = 1; //1 projectile is being added, no space in last ring, so add to new ring
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
            proj.holderVars = vars;
            proj.ringCapacity = vars.ringCapacity;
            proj.numInRing = vars.currentProjectilesInRing;
            proj.targetRadius = vars.ringRadius;
        }

        //========= Manage Remove Stack ==========
        public override void RemoveStack(Item item)
        {
            Item18Vars vars = item.vars as Item18Vars;
            //update data
            int planetsToRemove = item.stacks == 0 ? basePlanets : bonusPlanets;
            for (int i = 0; i < planetsToRemove; i++)
            {
                UpdateDamageData(vars);
                UpdateRemovedRingData(vars);
                RemoveProjectile(vars);
            }
            //delete effect check
            if (item.stacks == 0)
            {
                //destroy vfx
                Destroy(vars.vfx);
                //update vars
                vars.holder = null;
            }
        }

        private void UpdateRemovedRingData(Item18Vars vars)
        {
            vars.currentProjectilesInRing--;
            if (vars.currentProjectilesInRing < 0)
            {
                DecreaseRingIndex(vars);
            }
        }
        private void DecreaseRingIndex(Item18Vars vars)
        {
            vars.currentRingIndex--;
            vars.ringCapacity = GetRingCapacity(vars.currentRingIndex);
            vars.currentProjectilesInRing = vars.ringCapacity - 1; //1 projectile is being removed
            vars.ringRadius = GetRingRadius(vars.currentRingIndex);
        }

        private void RemoveProjectile(Item18Vars vars)
        {
            PlanetProjectile proj = vars.projectiles[^1]; //^1 == count - 1
            //unregister
            vars.projectiles.Remove(proj);
            //destroy object
            Destroy(proj.gameObject);
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"<color=#{HighlightColor}>+{basePlanets}</color> " +
                $"<color=#{StackColor}>(+{bonusPlanets} per stack)</color> " +
                $"orbiting planets which seek out " +
                $"<color=#{HighlightColor}>Nearby Enemies</color> " +
                $"dealing <color=#{HighlightColor}>{baseDamageMult * 100}%</color> " +
                $"damage";
        }
    }
}
