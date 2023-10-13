using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Core;
using UnityEngine.AI;
using Game.Core.GameSystems;
using Game.Core;
using System.Threading.Tasks;

namespace Game.Enemy {
    public class PGTaskGoToTarget : BT_Node
    {
        private Transform transform;
        private NavMeshAgent agent;
        private LayerMask LOSLayerMask;
        private bool attackingMelee = false;
        private Transform target;
        float distanceToTarget;

        public PGTaskGoToTarget(Transform transform, NavMeshAgent agent, LayerMask enemyLayerMask, Agent enemyAgent)
        {
            this.transform = transform;
            this.agent = agent;
            this.LOSLayerMask = enemyLayerMask;

            enemyAgent.health.onDeath.AddListener(CheckRangeOnDeath);
        }

        public override NodeState Evaluate()
        {
            //Check if target already was found. otherwise add it
            if (GetData("DistanceToTarget") == null) CheckDistance();
            if ((Transform)GetData("Target") == null) parent.SetData("Target", GameStateManager.instance.player.transform);
            target = (Transform)GetData("Target");
            
            if (GetData("DistanceToTarget") != null) 
            {
                distanceToTarget = (float)GetData("DistanceToTarget");
            }

            //If no target, fail
            if(target == null)
            {
                state = NodeState.FAILURE;
            }
            //else if mellee in ranged, success
            else if (distanceToTarget <= PGTree.meleeAttackRange)
            {
                HandleMelee();
                state = NodeState.SUCCESS;
            }
            //else if ranged in range && meleecounter > 3, success
            else if(distanceToTarget <= PGTree.rangedAttackRange && PGTree.EnemiesInRangeOfPlayer >= 3)
            {
                HandleRanged();
            }
            //else go to target
            else
            {
                if (distanceToTarget > PGTree.meleeAttackRange && attackingMelee)
                {
                    attackingMelee = false;
                    PGTree.EnemiesInRangeOfPlayer--;
                }
                agent.SetDestination(target.position);
                state = NodeState.RUNNING;
            }

            return state;
        }
        private void HandleMelee()
        {
            agent.SetDestination(transform.position);
            Vector3 targetPostition = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.LookAt(targetPostition);
            

            if (!attackingMelee)
            {
                attackingMelee = true;
                PGTree.EnemiesInRangeOfPlayer++;
            }
        }
        private void HandleRanged()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, target.position + Vector3.up - transform.position, out hit, Mathf.Infinity, LOSLayerMask))
            {
                if (hit.transform.tag == "Player")
                {
                    agent.SetDestination(transform.position);
                    Vector3 targetPostition = new Vector3(target.position.x, transform.position.y, target.position.z);
                    transform.LookAt(targetPostition);
                    state = NodeState.SUCCESS;
                }
                else
                {
                    state = NodeState.RUNNING;
                }
            }
        }
        void CheckRangeOnDeath(HitEvent hit)
        {
            if (attackingMelee)
            {
                attackingMelee = false;
                PGTree.EnemiesInRangeOfPlayer--;
            }
        }
    
        private async void CheckDistance()
        {
            while (transform != null)
            {
                await Task.Delay(2);
                if (target != null && transform != null) 
                {
                    parent.SetData("DistanceToTarget", Vector3.Distance(transform.position, target.position));
                }
            }
        }
    }
}
