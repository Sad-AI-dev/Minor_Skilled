using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [RequireComponent(typeof(MeshRenderer))]
    public class PlanetProjectile : MonoBehaviour
    {
        private enum State { Idle, Chase, Cooldown, Return }

        [Header("Movement Settings")]
        [SerializeField] private float baseMoveSpeed;
        [SerializeField] private float bonusMoveSpeed;
        [SerializeField] private float yOffset = 1f;

        [Header("Chase Settings")]
        [SerializeField] private SphereCollider detectSphere;
        [SerializeField] private float chaseSpeed = 2f;
        [SerializeField] private float maxChaseRange = 10f;

        [Header("Cooldown Settings")]
        [SerializeField] private float coolDown = 2f;

        [Header("Return Settings")]
        [SerializeField] private float returnTime = 0.5f;

        [Header("Visuals Settings")]
        [SerializeField] private List<Material> mats;

        //holder vars
        [HideInInspector] public Item18SO.Item18Vars holderVars;
        [HideInInspector] public int ringCapacity;
        [HideInInspector] public int numInRing;

        //movement settings
        [HideInInspector] public float targetRadius;

        //vars
        private float moveSpeed;
        private State state;

        private Agent target;

        //return vars
        private Vector3 startPos;
        private float timer;

        //================ Setup ===================
        private void Start()
        {
            SetupVisuals();
            InitializeVars();
        }

        private void SetupVisuals()
        {
            GetComponent<MeshRenderer>().material = mats[Random.Range(0, mats.Count)];
        }

        private void InitializeVars()
        {
            moveSpeed = baseMoveSpeed + (ringCapacity * bonusMoveSpeed);
            state = State.Idle;
            detectSphere.enabled = true;
        }

        //============== Update Loop =================
        private void Update()
        {
            switch (state)
            {
                case State.Idle: Move(); break;
                case State.Chase: Chase(); break;
                case State.Return: MoveToReturnPos(); break;
            }
        }

        //======= Move ======
        private void Move()
        {
            transform.localPosition = GetMovePos();
        }
        private Vector3 GetMovePos()
        {
            float fract = (((float)numInRing * (Mathf.PI * 2)) / (float)ringCapacity);
            float xPos = Mathf.Cos((Time.time * moveSpeed) + fract);
            float zPos = Mathf.Sin((Time.time * moveSpeed) + fract);
            return new Vector3(xPos, 0, zPos) * targetRadius + Vector3.up * yOffset;
        }

        //======= Chase ======
        private void Chase()
        {
            if (target && Vector3.Distance(transform.position, holderVars.holder.agent.transform.position) < maxChaseRange)
            {
                MoveToTarget();
            }
            else { SetState(State.Return); }
        }

        private void MoveToTarget()
        {
            Vector3 targetPos = target.transform.position + Vector3.up * 0.5f;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);
        }

        //======= CoolDown ======
        private void EndChase()
        {
            DealDamage();
            StartCoolDown();
        }
        private void DealDamage()
        {
            //setup hit event
            HitEvent hit = new HitEvent(holderVars.holder.agent, holderVars.procCoef);
            hit.baseDamage = holderVars.holder.agent.stats.baseDamage * holderVars.damageMult;
            //deal damage
            target.health.Hurt(hit);
        }

        private void StartCoolDown()
        {
            SetState(State.Cooldown);
            holderVars.holder.agent.StartCoroutine(CooldownCo());
        }
        private IEnumerator CooldownCo()
        {
            yield return new WaitForSeconds(coolDown);
            if (gameObject) { EndCoolDown(); } //make sure object still exists
        }

        private void EndCoolDown()
        {
            Move(); //correct Position
            SetState(State.Idle);
        }

        //========== Return State ============
        private void MoveToReturnPos()
        {
            timer += Time.deltaTime / returnTime;
            Vector3 targetPos = GetMovePos() + holderVars.holder.agent.transform.position;
            //update position
            transform.position = Vector3.Lerp(startPos, targetPos, timer);
            //done check
            if (Vector3.Distance(transform.position, targetPos) < 1f)
            {
                SetState(State.Idle);
            }
        }

        //============== Manage State Switching ==============
        private void SetState(State newState)
        {
            if (newState == state) { return; }
            state = newState;

            switch (state)
            {
                case State.Idle:
                    SetIdleState();
                    break;

                case State.Chase:
                    SetChaseState();
                    break;

                case State.Cooldown:
                    SetCooldownState();
                    break;

                case State.Return:
                    SetReturnState();
                    break;
            }
        }

        private void SetIdleState()
        {
            gameObject.SetActive(true);
            detectSphere.enabled = true;
            ResetTarget();
            transform.SetParent(holderVars.holder.agent.transform);
        }
        private void ResetTarget()
        {
            if (target)
            {
                target.health.onDeath.RemoveListener(CancelChase);
                target = null;
            }
        }

        private void SetChaseState()
        {
            gameObject.SetActive(true);
            detectSphere.enabled = false;
            transform.SetParent(null);
        }

        private void SetCooldownState()
        {
            gameObject.SetActive(false);
            detectSphere.enabled = false;
            ResetTarget();
        }

        private void SetReturnState()
        {
            gameObject.SetActive(true);
            detectSphere.enabled = true;
            ResetTarget();
            transform.SetParent(null);
            //setup return vars
            timer = 0f;
            startPos = transform.position;
        }

        //===== Handle Trigger Enter =====
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Agent agent))
            {
                if (agent == holderVars.holder.agent) { return; }
                if (state == State.Idle)
                {
                    SetChaseTarget(agent);
                }
                else if (state == State.Chase)
                {
                    if (agent != target) { target = agent; }
                    EndChase();
                }
            }
        }

        private void SetChaseTarget(Agent agent)
        {
            target = agent;
            target.health.onDeath.AddListener(CancelChase);
            SetState(State.Chase);
        }

        private void CancelChase(HitEvent hitEvent)
        {
            SetState(State.Return);
        }
    }
}
