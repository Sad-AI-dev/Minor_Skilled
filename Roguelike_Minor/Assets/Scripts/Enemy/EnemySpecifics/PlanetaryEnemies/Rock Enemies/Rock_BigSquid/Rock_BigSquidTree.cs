using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;
using Game.Core.GameSystems;

namespace Game.Enemy {
    public class Rock_BigSquidTree : Core.Tree
    {
        [Header("Other Components")]
        public LineRenderer lineRenderer;
        public CapsuleCollider meleeHitbox;
        public Animator anim;
        public Transform visuals;

        [Header("Attack Ranges")]
        public float RangedAttack = 30;
        public float MeleeAttack = 2.5f;
        public float LockOnAngle = 30;

        [Header("Rotation variable")]
        public float rotationSpeedTargeting = 360;

        [Header("Knockback Variables")]
        float MeleeKnockbackForce = 1;

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
                        new Rock_BigSquid_HandleRangeCheckRanged(transform, RangedAttack, agent, navAgent),
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
                if (root.GetData("MoveRotation") != null)
                {              
                    transform.rotation = Quaternion.Slerp(transform.rotation, (Quaternion)root.GetData("MoveRotation"), rotationSpeedTargeting / 100);
                }
            }
        }

        RaycastHit hit;
        Transform target;
        protected override void Update()
        {
            base.Update();

            lineRenderer.SetPosition(0, agent.abilities.primary.originPoint.position);
            if(root.GetData("RotationLeft") != null && (float)root.GetData("RotationLeft") > LockOnAngle)
            {
                if(Physics.Raycast(agent.abilities.primary.originPoint.position, agent.abilities.primary.originPoint.forward, out hit, 60))
                {
                    lineRenderer.SetPosition(1, hit.point);
                }
                else
                {
                    lineRenderer.SetPosition(1, agent.abilities.primary.originPoint.position + (agent.abilities.primary.originPoint.forward * 60));
                }
            }
            else
            {
                if(root.GetData("Target") != null)
                {
                    target = (Transform)root.GetData("Target");
                    lineRenderer.SetPosition(1, target.position + (Vector3.up * 0.5f));
                }
            }

            Align();
        }

        RaycastHit alignhHit;
        Vector3 theRay;

        private void Align()
        {
            theRay = -visuals.transform.up;

            if (Physics.Raycast(new Vector3(visuals.transform.position.x, visuals.transform.position.y, visuals.transform.position.z),
                theRay, out alignhHit, 2))
            {
                Debug.Log(alignhHit.transform.name);
                Quaternion targetRotation = Quaternion.FromToRotation(visuals.transform.up, alignhHit.normal) * visuals.transform.parent.rotation;

                visuals.transform.rotation = Quaternion.Lerp(visuals.transform.rotation, targetRotation, Time.deltaTime / 0.15f);
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

            yield return new WaitForSeconds(2f);

            navAgent.enabled = false;
            yield return null;
            navAgent.enabled = true;
        }   

        private void OnTriggerEnter(Collider other)
        {
            List<Agent> targets = new List<Agent>();
            Agent target;
            if (other.CompareTag("Player"))
            {
                target = other.GetComponent<Agent>();
                target.health.Hurt(new HitEvent(agent.abilities.secondary));
            }
            if(other.TryGetComponent<Agent>(out target))
            {
                targets.Add(target);
            }
            if(targets.Count > 0)
            {
                Explosion.DealKnockback(targets, MeleeKnockbackForce, transform.position);
            }
        }
    }
}
