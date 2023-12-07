using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy {
    public class Boss_BigSquid_ShotgunChance : BT_Node
    {
        int shootChance;
        float cooldownTime;

        bool canShoot = true;

        public Boss_BigSquid_ShotgunChance(Transform transform, Agent agent, int shootChance, float cooldownTime)
        {
            this.transform = transform;
            this.agent = agent;
            this.shootChance = shootChance;
            this.cooldownTime = cooldownTime;
        }

        public override NodeState Evaluate()
        {
            //Set Variables
            if(agent.abilities.secondary.vars == null)
            {
                agent.abilities.secondary.vars = new Boss_BigSquid_SecondaryVars
                {
                    target = (Transform)GetData("Target")
                };
            }

            state = NodeState.SUCCESS;
            if (canShoot)
            {
                int chance = Random.Range(1, 101);

                if (chance <= shootChance)
                {
                    agent.abilities.secondary.TryUse();
                }
                agent.StartCoroutine(DropCooldown());
            }

            return state;
        }

        IEnumerator DropCooldown()
        {
            canShoot = false;
            yield return new WaitForSeconds(cooldownTime);
            canShoot = true;
        }
    }
}
