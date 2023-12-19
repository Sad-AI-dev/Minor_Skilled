using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;
using Game.Core.Data;
using System.Threading.Tasks;

namespace Game.Enemy
{
    public class Boss_BigSquid_SpawnMinionCheck : BT_Node
    {
        //Variables
        int minionSpawnChancePercent;
        float minionSpawnChanceCooldownTime;
        bool canCheck = true;
        WeightedChance<GameObject> enemyList;
        Vector2Int spawnAmountMinMax;



        public Boss_BigSquid_SpawnMinionCheck(
            Agent agent, Transform transform, int minionSpawnChancePercent, 
            WeightedChance<GameObject> enemyList, float minionSpawnChanceCooldownTime, Vector2Int spawnAmountMinMax)
        {
            this.minionSpawnChancePercent = minionSpawnChancePercent;
            this.agent = agent;
            this.enemyList = enemyList;
            this.minionSpawnChanceCooldownTime = minionSpawnChanceCooldownTime;
            this.transform = transform;
            this.spawnAmountMinMax = spawnAmountMinMax;
        }

        public override NodeState Evaluate()
        {
            if (canCheck)
            {
                int chance = Random.Range(0, 100);
                if(chance <= minionSpawnChancePercent)
                {
                    int amountOfEnemies = Random.Range(spawnAmountMinMax.x, spawnAmountMinMax.y+1);
                    agent.StartCoroutine(SpawnMinions(amountOfEnemies));
                }
                agent.StartCoroutine(CanCheckCooldown());
            }

            state = NodeState.SUCCESS;
            return state;
        }
        IEnumerator CanCheckCooldown()
        {
            canCheck = false;
            yield return new WaitForSeconds(minionSpawnChanceCooldownTime);
            canCheck = true;
        }

        IEnumerator SpawnMinions(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Object.Instantiate(enemyList.GetRandomEntry(), transform.position, Quaternion.identity);
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
