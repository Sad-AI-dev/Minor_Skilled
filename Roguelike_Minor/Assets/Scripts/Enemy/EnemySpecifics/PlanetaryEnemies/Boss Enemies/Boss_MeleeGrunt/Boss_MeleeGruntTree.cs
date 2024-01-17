using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;
using Game.Core.Data;

namespace Game.Enemy
{
    public class Boss_MeleeGruntTree : Core.Tree
    {
        public static float EarthQuakeRange = 40;
        public static float ClapRange = 10;
        public static float GroundSlamRange = 20;

        [Header("Boss Melee Grunt Specific Variables")]
        public float earthQuakeRange = 40;
        public float clapRange = 10;
        public float groundSlamRange = 20;

        public Dictionary<Ability, bool> activeAbilities = new Dictionary<Ability, bool>();
        public Animator anim;
        public GameObject clapHitbox;

        public Vector2Int timeBetweenAbilitiesMinMax;

        protected override void Awake()
        {
            agent.abilities.secondary.vars = new Boss_MeleeGrunt_SecondaryAttackVars()
            {
                Anim = anim,
                ClapHitbox = clapHitbox
            };

            base.Awake();
        }

        protected override void Start()
        {
            EarthQuakeRange = earthQuakeRange;
            ClapRange = clapRange;
            GroundSlamRange = groundSlamRange;

            navAgent.speed = agent.stats.walkSpeed;

            activeAbilities.Add(agent.abilities.primary, false);
            activeAbilities.Add(agent.abilities.secondary, false);
            activeAbilities.Add(agent.abilities.special, false);

            base.Start();
        }

        protected override BT_Node SetupTree()
        {
            root = new Sequence(
                new List<BT_Node>
                {
                    //Walk to target
                    new Boss_MeleeGrunt_MoveToTarget(transform, agent, navAgent),

                    //If in range
                    new Sequence(
                        new List<BT_Node>
                        {
                            new Boss_MeleeGrunt_CheckRange(transform, agent, this),
                            new Boss_MeleeGrunt_ChooseAbility(agent, this)
                        })
                });

            return root;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, earthQuakeRange);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, clapRange);

            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, groundSlamRange);
        }

        public void FinishedAbility()
        {
            root.SetData("UsingAbility", false);
            StartCoroutine(AbilityCooldownCo());
        }
        IEnumerator AbilityCooldownCo()
        {
            root.SetData("OnCooldown", true);
            yield return new WaitForSeconds(Random.Range(timeBetweenAbilitiesMinMax.x, timeBetweenAbilitiesMinMax.y));
            root.SetData("OnCooldown", false);
        }
    }
}

