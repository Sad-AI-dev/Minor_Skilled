using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "24Firework_Champaign", menuName = "ScriptableObjects/Items/T2/24: Firework Champaign", order = 224)]
    public class Item24SO : ItemDataSO
    {
        private class Item24Vars : Item.ItemVars
        {
            public float chestLuck;

            public float GetChestLuck()
            {
                return chestLuck / (chestLuck + 0.5f);
            }
        }

        [Header("Chest Settings")]
        public GameObject chestPrefab;

        [Header("Chest Luck settings")]
        public float baseChestLuck = 0.2f;
        public float bonusChestLuck = 0.2f;

        //======== Initialize Vars ==========
        public override void InitializeVars(Item item)
        {
            item.vars = new Item24Vars();
            //listen to events
            EventBus<GameEndEvent>.AddListener(HandleGameEnd);
            EventBus<SceneLoadedEvent>.AddListener(HandleSceneLoad);
        }

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            Item24Vars vars = item.vars as Item24Vars;
            if (item.stacks == 1) { vars.chestLuck += baseChestLuck; }
            else { vars.chestLuck += bonusChestLuck; }
        }

        public override void RemoveStack(Item item)
        {
            Item24Vars vars = item.vars as Item24Vars;
            if (item.stacks == 0) 
            { 
                vars.chestLuck -= baseChestLuck;
                HandleGameEnd(null);
            }
            else { vars.chestLuck -= bonusChestLuck; }
        }

        //======== Handle Scene Load ========
        private void HandleSceneLoad(SceneLoadedEvent eventData)
        {
            GameStateManager.instance.StartCoroutine(PlaceChestCo());
        }

        private IEnumerator PlaceChestCo()
        {
            if (!GameStateManager.instance.scalingIsPaused)
            {
                yield return null; //wait on lootplacer assignment
                PlaceChest();
            }
        }

        private void PlaceChest()
        {
            GameObject chest = Instantiate(chestPrefab);
            GameStateManager.instance.lootSpawner.PlaceObject(chest);
            //setup chest
            Agent player = GameStateManager.instance.player;
            Item24Vars vars = player.inventory.GetItemOfType(this).vars as Item24Vars;
            chest.GetComponent<Lootable>().lootLuck = vars.GetChestLuck();
        }

        //======= Handle Game End =====
        private void HandleGameEnd(GameEndEvent eventData)
        {
            EventBus<GameEndEvent>.RemoveListener(HandleGameEnd);
            EventBus<SceneLoadedEvent>.RemoveListener(HandleSceneLoad);
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"Spawn a <color=#{HighlightColor}>Unique Chest</color> on the next stage " +
                $"with a <color=#{HighlightColor}>{baseChestLuck * 100}%</color> " +
                $"<color=#{StackColor}>(+{bonusChestLuck * 100}% per stack)</color> " +
                $"chance to contain a <color=#{HighlightColor}>Higher Rarity</color> item";
        }
    }
}
