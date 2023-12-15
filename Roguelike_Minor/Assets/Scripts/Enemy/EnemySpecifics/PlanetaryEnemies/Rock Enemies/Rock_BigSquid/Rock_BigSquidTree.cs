using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy {
    public class Rock_BigSquidTree : Core.Tree
    {
        [Header("Other Components")]
        public LineRenderer lineRenderer;
        public CapsuleCollider meleeHitbox;
        public Animator anim;

        [Header("Attack Ranges")]
        public float RangedAttack = 30;
        public float MeleeAttack = 2.5f;

        [Header("Rotation variable")]
        float rotationSpeedTargeting = 360;

        [Header("Knockback Variables")]
        public float upMultiplier;
        public float directionMultiplier;

        protected override void Start()
        {
            agent.abilities.primary.vars = new BigSquidPrimaryVars
            {
                target = null,
                lineRenderer = this.lineRenderer,
                targetingCo = null,
                root = this.root
            };
            agent.abilities.secondary.vars = new Rock_BigSquid_SecondaryAttackVars
            {
                anim = this.anim
            };

            base.Start();
        }

        protected override BT_Node SetupTree()
        {
            BT_Node root = new Selector(
                new List<BT_Node>
                {
                    new TakeKnockback(transform, agent, navAgent, rb, upMultiplier, directionMultiplier),

                    //Check range of short ranged attack
                    new Sequence(new List<BT_Node>
                    {
                        new CheckRangeNode(transform, MeleeAttack),
                        new Rock_BigSquid_HandleMelee(agent)

                    }),
                    //check range of ranged attack
                    new Sequence(new List<BT_Node>
                    {
                        new CheckRangeNode(transform, RangedAttack),
                        new Rock_BigSquid_HandleRanged(transform, agent, lineRenderer, navAgent, rotationSpeedTargeting)

                    }),
                    //Move closer
                    new Rock_BigSquid_MoveToTarget(navAgent)
                }
            );
            return root;
        }

        void FixedUpdate()
        {
            if (root != null)
            {
                if (root.GetData("MoveRotation") != null) rb.MoveRotation((Quaternion)root.GetData("MoveRotation"));
            }
        }
        public void ActivateHitboxMelee()
        {
            StartCoroutine(DeactivatorCo());
        }
        IEnumerator DeactivatorCo()
        {
            meleeHitbox.enabled = true;
            yield return new WaitForSeconds(0.05f);
            meleeHitbox.enabled = false;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Agent>().health.Hurt(new HitEvent(agent.abilities.secondary));
            }
        }
    }
}
