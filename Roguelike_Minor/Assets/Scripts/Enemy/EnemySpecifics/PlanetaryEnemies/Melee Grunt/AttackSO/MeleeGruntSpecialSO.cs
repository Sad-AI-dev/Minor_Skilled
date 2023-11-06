using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.Data;
using System;
using UnityEngine.AI;

namespace Game.Enemy
{
    [CreateAssetMenu(fileName = "MeleeGruntSpecial", menuName = "ScriptableObjects/Enemy/MeleeGrunt/Special")]
    public class MeleeGruntSpecialSO : AbilitySO
    {
        [SerializeField] private float chargeWindUpTimer;

        //private PlayerController controller;

        public class MeleeGruntSpecialVars : Ability.AbilityVars
        {
            public MeleeGruntTree enemy;
            public Vector3 target;
            public NavMeshAgent navAgent;
            public Agent agent;
            public Coroutine chargeCO; 
        }

        public override void InitializeVars(Ability source)
        {
            source.agent.GetComponent<MeleeGruntTree>().OnHitPlayer += HitTarget;
            source.vars = new MeleeGruntSpecialVars
            {
                enemy = source.agent.GetComponent<MeleeGruntTree>(),
                navAgent = source.agent.GetComponent<NavMeshAgent>(),
                agent = source.agent,
                target = GameStateManager.instance.player.transform.position,
                chargeCO = null 
            };
        }

        public override void Use(Ability source)
        {
            MeleeGruntSpecialVars vars = (source.vars as MeleeGruntSpecialVars);
            Vector3 playerDirection = vars.target - vars.enemy.transform.position;
            Vector3 velocity = playerDirection;
            vars.chargeCO = source.agent.StartCoroutine(ChargeCO(vars, velocity));
        }

        public IEnumerator ChargeCO(MeleeGruntSpecialVars vars, Vector3 dir)
        {
            vars.navAgent.velocity = Vector3.zero;

            yield return new WaitForSeconds(chargeWindUpTimer);

            vars.navAgent.velocity = dir * 8;

            vars.navAgent.speed = 10;
            vars.navAgent.angularSpeed = 0;
            vars.navAgent.acceleration = 20;

            yield return new WaitForSeconds(0.2f);

            vars.navAgent.speed = vars.agent.stats.walkSpeed;
            vars.navAgent.angularSpeed = 180;
            vars.navAgent.acceleration = 10;
        }

        void HitTarget(MeleeGruntTree tree)
        {
            tree.navAgent.velocity = Vector3.zero;
            tree.navAgent.speed = tree.agent.stats.walkSpeed;
            tree.navAgent.angularSpeed = 180;
            tree.navAgent.acceleration = 10;

            GameStateManager.instance.transform.GetComponent<Agent>().health.Hurt(new HitEvent(tree.transform.GetComponent<Agent>().abilities.special));
        }
    }
}
