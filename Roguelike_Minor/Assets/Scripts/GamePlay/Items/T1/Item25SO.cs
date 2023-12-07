using UnityEngine;
using Game.Core;
using Game.Util;
using System.Collections;

namespace Game {
    [CreateAssetMenu(fileName = "25Liquid_Clovers", menuName = "ScriptableObjects/Items/T1/25: Liquid Clovers", order = 125)]
    public class Item25SO : ItemDataSO
    {
        private class Item25Vars : Item.ItemVars
        {
            public float chance;
        }

        [Header("Spawn chance settings")]
        public float baseChance = 20f;
        public float bonusChance = 20f;

        [Header("Chest Settings")]
        public GameObject chestPrefab;

        //========= Initialize Vars ============
        public override void InitializeVars(Item item)
        {
            item.vars = new Item25Vars();
        }

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            Item25Vars vars = item.vars as Item25Vars;
            if (item.stacks == 1) 
            { 
                vars.chance += baseChance;
                EventBus<SceneLoadedEvent>.AddListener(OnSceneLoad);
                EventBus<GameEndEvent>.AddListener(OnGameEnd);
            }
            else { vars.chance += bonusChance; }
        }

        public override void RemoveStack(Item item)
        {
            Item25Vars vars = item.vars as Item25Vars;
            if (item.stacks == 0) 
            { 
                vars.chance -= baseChance;
                EventBus<SceneLoadedEvent>.RemoveListener(OnSceneLoad);
                EventBus<GameEndEvent>.RemoveListener(OnGameEnd);
            }
            else { vars.chance -= bonusChance; }
        }

        //========= Handle Events ===========
        private void OnSceneLoad(SceneLoadedEvent eventData)
        {
            GameStateManager.instance.StartCoroutine(TrySpawnChestsCo());
        }

        private void OnGameEnd(GameEndEvent eventData)
        {
            EventBus<SceneLoadedEvent>.RemoveListener(OnSceneLoad);
            EventBus<GameEndEvent>.RemoveListener(OnGameEnd);
        }

        //======== Spawn Additional Chests ============
        private IEnumerator TrySpawnChestsCo()
        {
            if (!GameStateManager.instance.scalingIsPaused) 
            {
                yield return null;
                Agent player = GameStateManager.instance.player;
                Item25Vars vars = player.inventory.GetItemOfType(this).vars as Item25Vars;
                AgentRandom.TryProc(vars.chance, player, SpawnChest);
            }
        }

        private void SpawnChest()
        {
            LootSpawner spawner = GameStateManager.instance.lootSpawner;
            if (spawner)
            {
                spawner.PlaceLoot(chestPrefab);
            }
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"Gain a <color=#{HighlightColor}>{baseChance}%</color> " +
                $"<color=#{StackColor}>(+{bonusChance}% per stack)</color> " +
                $"chance to spawn a <color=#{HighlightColor}>chest</color> " +
                $"on the next planet";
        }
    }
}
