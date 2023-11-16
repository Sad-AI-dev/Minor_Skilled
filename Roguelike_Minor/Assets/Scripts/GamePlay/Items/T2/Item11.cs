using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "11Smelling_Salts", menuName = "ScriptableObjects/Items/T2/11: Smelling Salts", order = 211)]
    public class Item11 : ItemDataSO
    {
        [Header("MoveSpeed settings")]
        public int baseUses = 1;
        public int bonusUses = 1;

        //========= Manage Stacks ===========
        public override void AddStack(Item item)
        {
            if (item.stacks == 1) { item.agent.abilities.utility.GainMaxUses(baseUses); }
            else { item.agent.abilities.utility.GainMaxUses(bonusUses); }
        }

        public override void RemoveStack(Item item)
        {
            if (item.stacks == 0) { item.agent.abilities.utility.RemoveMaxUses(baseUses); }
            else { item.agent.abilities.utility.RemoveMaxUses(bonusUses); }
        }

        //========== Description ===========
        public override string GenerateLongDescription()
        {
            return $"Gain <color=#{HighlightColor}>{baseUses} use</color> " +
                $"<color=#{StackColor}>(+{bonusUses} use per stack)</color> " +
                $"of your Utility ability";
        }
    }
}
