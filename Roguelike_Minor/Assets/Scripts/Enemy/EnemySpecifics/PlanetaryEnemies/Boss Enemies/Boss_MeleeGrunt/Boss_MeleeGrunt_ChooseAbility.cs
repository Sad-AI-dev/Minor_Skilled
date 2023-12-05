using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;
using Game.Core.Data;


namespace Game.Enemy
{
    public class Boss_MeleeGrunt_ChooseAbility : BT_Node
    {
        Boss_MeleeGruntTree tree;
        List<Ability> abilities = new List<Ability>();

        public Boss_MeleeGrunt_ChooseAbility(Agent agent, Boss_MeleeGruntTree tree)
        {
            this.agent = agent;
            this.tree = tree;
        }

        public override NodeState Evaluate()
        {
            state = NodeState.SUCCESS;

            foreach (var ability in tree.activeAbilities)
            {
                if(ability.Value)
                {
                    if (!abilities.Contains(ability.Key))
                    {
                        abilities.Add(ability.Key);
                    }
                }
            }

            if (GetData("OnCooldown") == null || !(bool)GetData("OnCooldown"))
            {
                if (abilities.Count > 0)
                {
                    abilities[Random.Range(0, abilities.Count)].TryUse();
                    SetData("UsingAbility", true);
                }
            }

            return state;
        }
    }
}
