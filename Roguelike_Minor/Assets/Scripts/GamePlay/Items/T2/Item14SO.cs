using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "14Shield^3", menuName = "ScriptableObjects/Items/T2/14: Shield^3", order = 214)]
    public class Item14SO : ItemDataSO, ITakeDamageProcessor
    {
        private class Item14Vars : Item.ItemVars
        {
            //holder
            public Item owner;
            //shield vars
            public float maxShield;
            public float currentShield;
            //downed vars
            public bool isRecharging;
            public float rechargeTime;
            //visuals
            public GameObject shieldVisuals;

            //========= Manage Max Shield ========
            public void AddShield(float toAdd)
            {
                maxShield += toAdd;
                if (!isRecharging) { currentShield += toAdd; }
                UpdateShield();
            }
            public void RemoveShield(float toRemove)
            {
                maxShield -= toRemove;
                if (maxShield <= 0f) { Destroy(); }
                else { UpdateShield(); }
            }
            private void Destroy()
            {
                Object.Destroy(shieldVisuals); //remove visuals
            }

            //======= Absorb Damage ========
            public void AbsorbDamage(float damageToAbsorb)
            {
                currentShield -= damageToAbsorb;
                UpdateShield();
            }

            //======= Handle Shield state logic =======
            private void UpdateShield()
            {
                bool isDown = currentShield <= 0f;
                currentShield = Mathf.Clamp(currentShield, 0f, maxShield);
                if (isDown)
                {
                    shieldVisuals.SetActive(false);
                    owner.agent.StartCoroutine(RechargeCo());
                }
            }

            private IEnumerator RechargeCo()
            {
                isRecharging = true;
                yield return new WaitForSeconds(rechargeTime);
                shieldVisuals.SetActive(true);
                currentShield = maxShield;
                isRecharging = false;
            }
        }

        [Header("Priority")]
        public int priority;
        public int GetPriority() { return priority; }

        [Header("Shield vars")]
        public float baseShield;
        public float bonusShield;

        [Header("Recharge vars")]
        public float rechargeTime;

        [Header("Visuals")]
        public GameObject visualsPrefab;

        //========= Initialize Vars ===========
        public override void InitializeVars(Item item)
        {
            item.vars = new Item14Vars()
            {
                owner = item,
                shieldVisuals = Instantiate(visualsPrefab, item.agent.transform),
                rechargeTime = rechargeTime
            };
        }

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            Item14Vars vars = item.vars as Item14Vars;
            if (item.stacks == 1) { vars.AddShield(baseShield); }
            else { vars.AddShield(bonusShield); }
        }

        public override void RemoveStack(Item item)
        {
            Item14Vars vars = item.vars as Item14Vars;
            if (item.stacks == 0) { vars.RemoveShield(baseShield); }
            else { vars.RemoveShield(bonusShield); }
        }

        //========== Process Take Damage ===============
        public void ProcessTakeDamage(ref HitEvent hitEvent, Item sourceItem)
        {
            Item14Vars vars = sourceItem.vars as Item14Vars;
            //all damage was blocked
            if (vars.currentShield > hitEvent.GetTotalDamage())
            {
                hitEvent.blocked = true;
                vars.AbsorbDamage(hitEvent.GetTotalDamage());
            }
            //shield is down / not all damage blocked
            else
            {
                hitEvent.damageReduction = vars.currentShield;
                vars.AbsorbDamage(vars.currentShield + 1f); //ensure shield goes down
            }
        }
        public void ProcessTakeDamage(ref HitEvent hitEvent, List<StatusEffectHandler.EffectVars> vars) { }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"Gain a <color=#{HighlightColor}>shield</color> " +
                $"that absorbs <color=#{HighlightColor}>{baseShield} hp</color> " +
                $"<color=#{StackColor}>(+{bonusShield} hp per stack)</color> " +
                $"before <color=#{StackColor}>recharging</color>";
        }
    }
}
