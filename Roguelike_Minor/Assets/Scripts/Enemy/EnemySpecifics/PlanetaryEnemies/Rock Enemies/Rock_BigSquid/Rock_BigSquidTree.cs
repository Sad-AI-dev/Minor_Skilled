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
            base.Start();
        }

        protected override BT_Node SetupTree()
        {
            root = new Selector(
                new List<BT_Node>
                {
                    new TakeKnockback(transform, agent, rb, upMultiplier, directionMultiplier, navAgent),

                    //Check range of short ranged attack
                    new Sequence(new List<BT_Node>
                    {
                        new CheckRangeNode(agent, MeleeAttack),
                        new UseSecondaryNode(agent)

                    }),
                    //check range of ranged attack
                    new Sequence(new List<BT_Node>
                    {
                        new Rock_BigSquid_HandleRangeCheckRanged(transform, RangedAttack, agent, rotationSpeedTargeting, navAgent),
                        new UsePrimaryNode(agent)

                    }),
                    //Move closer
                    new MoveToTargetNavMeshNode(navAgent, agent)
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

        protected override void Update()
        {
            base.Update();
            lineRenderer.SetPosition(0, agent.abilities.primary.originPoint.position);
            lineRenderer.SetPosition(1, agent.abilities.primary.originPoint.position + (agent.abilities.primary.originPoint.forward * 60));
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
