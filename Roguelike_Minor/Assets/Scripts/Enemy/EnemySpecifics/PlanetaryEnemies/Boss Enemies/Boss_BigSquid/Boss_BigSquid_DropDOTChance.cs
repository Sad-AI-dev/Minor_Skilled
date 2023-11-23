using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy {
    public class Boss_BigSquid_DropDOTChance : BT_Node
    {
        Transform transform;
        Agent agent;
        int goopDropChance;
        float goopChanceCooldown;

        bool canDrop = true;

        public Boss_BigSquid_DropDOTChance(Transform transform, Agent agent, int goopDropChance, float goopChanceCooldown)
        {
            this.transform = transform;
            this.agent = agent;
            this.goopChanceCooldown = goopChanceCooldown;
            this.goopDropChance = goopDropChance;
        }

        public override NodeState Evaluate()
        {
            if (canDrop)
            {
                int chance = Random.Range(1, 101);

                if(chance <= goopDropChance)
                {
                    agent.abilities.primary.TryUse();
                }
                agent.StartCoroutine(DropCooldown());
            }

            return state;
        }

        IEnumerator DropCooldown()
        {
            canDrop = false;
            yield return new WaitForSeconds(goopChanceCooldown);
            canDrop = true;
        }
    }
}
