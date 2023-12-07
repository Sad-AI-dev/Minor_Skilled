using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.Data;
using System;
using UnityEngine.AI;

namespace Game.Enemy {
    [CreateAssetMenu(fileName = "MeleeGruntSpecial", menuName = "ScriptableObjects/Enemy/MeleeGrunt/Special")]
    public class MeleeGruntSpecialSO : AbilitySO
    {
        [SerializeField] private float chargeWindUpTimer;
        [SerializeField] private float chargeSpeed;

        public class MeleeGruntSpecialVars : Ability.AbilityVars
        {
            public MeleeGruntTree enemy;
            public Transform target;
            public NavMeshAgent navAgent;
            public Agent agent;
            public Coroutine chargeCO;
            public Animator anim;
        }

        public override void InitializeVars(Ability source)
        {
            source.agent.GetComponent<MeleeGruntTree>().OnHitPlayer += HitTarget;
            source.vars = new MeleeGruntSpecialVars
            {
                enemy = source.agent.GetComponent<MeleeGruntTree>(),
                navAgent = source.agent.GetComponent<NavMeshAgent>(),
                agent = source.agent,
                target = GameStateManager.instance.player.transform,
                chargeCO = null,
                anim = source.agent.GetComponent<Animator>()
            };
        }

        public override void Use(Ability source)
        {
            MeleeGruntSpecialVars vars = (source.vars as MeleeGruntSpecialVars);
            vars.anim.SetTrigger("Charge");
            vars.chargeCO = source.agent.StartCoroutine(ChargeCO(vars));
        }

        public IEnumerator ChargeCO(MeleeGruntSpecialVars vars)
        {
            vars.enemy.charging = true;
            vars.navAgent.velocity = Vector3.zero;

            yield return new WaitForSeconds(chargeWindUpTimer);

            Dash(vars);

            yield return new WaitForSeconds(0.3f);

            StopDash(vars);

            yield return new WaitForSeconds(1);

            vars.enemy.charging = false;
        }

        void Dash(MeleeGruntSpecialVars vars)
        {
            Vector3 playerDirection = vars.target.position - vars.enemy.transform.position;
            Vector3 velocity = playerDirection * chargeSpeed;
            vars.navAgent.isStopped = false;
            vars.navAgent.velocity = velocity;

            vars.navAgent.speed = 10;
            vars.navAgent.angularSpeed = 0;
            vars.navAgent.acceleration = 20;
        }
        void StopDash(MeleeGruntSpecialVars vars)
        {
            vars.navAgent.velocity = Vector3.zero;
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

            GameStateManager.instance.player.health.Hurt(new HitEvent(tree.transform.GetComponent<Agent>().abilities.special));
        }
    }
}
