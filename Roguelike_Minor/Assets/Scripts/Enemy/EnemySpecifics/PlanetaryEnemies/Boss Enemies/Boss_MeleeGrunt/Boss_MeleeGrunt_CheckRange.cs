using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;
using Game.Core.Data;


namespace Game.Enemy {
    public class Boss_MeleeGrunt_CheckRange : BT_Node
    {
        float distance;
        Boss_MeleeGruntTree tree;

        public Boss_MeleeGrunt_CheckRange(Transform transform, Agent agent, Boss_MeleeGruntTree tree)
        {
            this.transform = transform;
            this.agent = agent;
            this.tree = tree;
        }

        public override NodeState Evaluate()
        {
            state = NodeState.FAILURE;
            if (GetData("Target") == null) SetTarget(GameStateManager.instance.player.transform);
            target = (Transform)GetData("Target");

            if (GetData("DistanceToTarget") == null) agent.StartCoroutine(DistanceToTargetCO(agent, transform, target));
            distance = (float)GetData("DistanceToTarget");

            if (target != null)
            {
                //Check EQ
                CheckRange(Boss_MeleeGruntTree.EarthQuakeRange, agent.abilities.primary);
                CheckRange(Boss_MeleeGruntTree.ClapRange, agent.abilities.secondary);
                CheckRange(Boss_MeleeGruntTree.GroundSlamRange, agent.abilities.special);
            }
            
            return state;
        }

        void CheckRange(float Range, Ability ability)
        {
            if (distance <= Range)
            {
                tree.activeAbilities[ability] = true;
                state = NodeState.SUCCESS;
            }
            else tree.activeAbilities[ability] = false;
        }
    }
}
