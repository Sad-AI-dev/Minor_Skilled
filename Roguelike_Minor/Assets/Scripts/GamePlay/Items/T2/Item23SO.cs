using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "23Efficiency_Bonus", menuName = "ScriptableObjects/Items/T2/23: Efficiency Bonus", order = 223)]
    public class Item23SO : ItemDataSO
    {
        private class Item23Vars : Item.ItemVars
        {
            public int baseMoney;
        }

        [Header("Money Settings")]
        public int baseMoney;
        public int bonusMoney;
        [Space(10f)]
        public float maxMult = 5f;
        public float priceScaleMult = 0.5f;

        [Header("Timing Settings")]
        public float maxDuration = 300f;
        public AnimationCurve multCurve;

        //static timer
        private float stageEnterTime = -1f;

        //========= Initialize Vars ===========
        public override void InitializeVars(Item item)
        {
            item.vars = new Item23Vars();
            //initialize listeners
            EventBus<GameEndEvent>.AddListener(HandleGameEnd);
            EventBus<SceneLoadedEvent>.AddListener(HandleStageLoad);
        }

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            Item23Vars vars = item.vars as Item23Vars;
            if (item.stacks == 1) { vars.baseMoney += baseMoney; }
            else { vars.baseMoney += bonusMoney; }
        }

        public override void RemoveStack(Item item)
        {
            Item23Vars vars = item.vars as Item23Vars;
            if (item.stacks == 0) 
            { 
                vars.baseMoney -= baseMoney;
                HandleGameEnd(null);
            }
            else { vars.baseMoney -= bonusMoney; }
        }

        //===== Handle Stage Load ====
        private void HandleStageLoad(SceneLoadedEvent eventData)
        {
            //stage completed, reward money
            RewardMoney();
            //start new timer check
            if (!GameStateManager.instance.scalingIsPaused) {
                //record time
                stageEnterTime = UITimeManager.currentTime;
            }
        }

        private void RewardMoney()
        {
            GameStateManager.instance.player.stats.Money += Mathf.FloorToInt(GetBaseMoney() * GetMultiplier());
        }

        private float GetMultiplier()
        {
            float stageDuration = UITimeManager.currentTime - stageEnterTime;
            if (stageDuration > maxDuration) { return 1f; }
            else
            { //calculate multiplier based on stage duration
                float durationPercent = stageDuration / maxDuration;
                return (multCurve.Evaluate(durationPercent) * (maxMult - 1f)) + 1f;
            }
        }

        private float GetBaseMoney()
        {
            Item sourceItem = GameStateManager.instance.player.inventory.GetItemOfType(this);
            return (sourceItem.vars as Item23Vars).baseMoney * GetPriceScaleMult();
        }
        private float GetPriceScaleMult()
        {
            return GameScalingManager.instance.priceMult * priceScaleMult;
        }

        //===== Handle Game End =====
        private void HandleGameEnd(GameEndEvent eventData)
        {
            EventBus<GameEndEvent>.RemoveListener(HandleGameEnd);
            EventBus<SceneLoadedEvent>.RemoveListener(HandleStageLoad);
            //reset vars
            stageEnterTime = -1f;
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"Earn <color=#{HighlightColor}>{baseMoney} money</color> " +
                $"<color=#{StackColor}>(+{bonusMoney} money per stack)</color> on stage completion\n" +
                $"Earn up to <color=#{HighlightColor}>{maxMult * 100}%</color> " +
                $"additional money based on " +
                $"<color=#{HighlightColor}>completion time</color>\n" +
                $"Gives more money over time.";
        }
    }
}
