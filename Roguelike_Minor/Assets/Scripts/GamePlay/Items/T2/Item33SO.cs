using Game.Core;
using Game.Core.GameSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using static Game.Item15SO;

namespace Game
{
    [CreateAssetMenu(fileName = "33Killer_Tail", menuName = "ScriptableObjects/Items/T2/33: Killer Tail", order = 233)]
    public class Item33SO : ItemDataSO
    {
        public class KillerItemitemVars : Item.ItemVars
        {
            public float damageMultiplier;
        }

        [Header("Proc Chance Settings")]
        public float procChance = 50f;

        [Header("Damage")]
        [SerializeField] private float baseDamageMult;
        [SerializeField] private float bonusDamageMult;

        [Header("Missile")]
        [SerializeField] private GameObject missile;

        public override void InitializeVars(Item item)
        {
            item.vars = new KillerItemitemVars { damageMultiplier = 0 };
        }

        public override void AddStack(Item item)
        {
            KillerItemitemVars vars = item.vars as KillerItemitemVars;
            if (item.stacks == 1) { vars.damageMultiplier += baseDamageMult; }
            else { vars.damageMultiplier += bonusDamageMult; }
        }

        public override void RemoveStack(Item item)
        {
            KillerItemitemVars vars = item.vars as KillerItemitemVars;
            if (item.stacks == 0) { vars.damageMultiplier -= baseDamageMult; }
            else { vars.damageMultiplier -= bonusDamageMult; }
        }

        public override void ProcessDealDamage(ref HitEvent hitEvent, Item sourceItem)
        {
            if (!hitEvent.hasAgentSource) return;
            AgentRandom.TryProc(procChance, hitEvent, FireMissile, hitEvent);
        }

        private void FireMissile(HitEvent hitEvent)
        {
            SeekingMissile missileScript = Instantiate(missile, hitEvent.source.transform.position + Vector3.up, Quaternion.identity).GetComponent<SeekingMissile>();

            Item item = hitEvent.source.inventory.GetItemOfType(this);

            HitEvent newHitEvent = new HitEvent(hitEvent, item);
            newHitEvent.baseDamage = hitEvent.GetTotalDamage() * (item.vars as KillerItemitemVars).damageMultiplier;
            missileScript.InitializeVars(newHitEvent);
        }

        public override string GenerateLongDescription()
        {
            return "balls";
        }
    }
}
