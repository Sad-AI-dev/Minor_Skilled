using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Game.Core
{
    [RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float lifeTime;
        [SerializeField] private LayerMask layermask;

        [HideInInspector] public Vector3 velocity;
        [HideInInspector] public Ability ability;
        protected Agent source;
        private string sourceTag;
        private float baseDamage;

        private Rigidbody rb;
        private SphereCollider col;

        //========== Initialize ===========
        private void OnEnable()
        {
            col = GetComponent<SphereCollider>();
            col.isTrigger = true;
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            StartCoroutine(LifeTimeCo());
        }

        public void Initialize(Ability source)
        {
            ability = source;
            InitializeSourceVars();
            //subclass initialization
            InitializeVars();
        }

        private void InitializeSourceVars()
        {
            this.source = ability.agent;
            sourceTag = source.tag;
            baseDamage = source.stats.baseDamage * ability.abilityData.damageMultiplier;
        }

        protected virtual void InitializeVars()
        {

        }

        //========== Update ===============
        private void FixedUpdate()
        {
            //CheckHitObject();
            UpdateMoveDir();
            rb.velocity = velocity;
        }

        //=========== Collision ==============
        private void CheckHitObject()
        {
            Vector3 transformPos = transform.position - new Vector3(0, 0, transform.localScale.z / 2);
            RaycastHit hit;
            if (Physics.Raycast(transformPos, velocity, out hit, velocity.magnitude, layermask, QueryTriggerInteraction.Ignore))
            {
                if (!hit.transform.CompareTag(sourceTag))
                {
                    //Debug.Log(hit.transform.name);
                    OnCollide(hit);
                    gameObject.SetActive(false);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag(sourceTag))
            {
                CustomCollide(other);
                gameObject.SetActive(false);
            }
        }

        protected virtual void CustomCollide(Collider other)
        {

        }

        protected virtual void OnCollide(RaycastHit hit)
        {

        }

        //============ Deal Damage =============
        protected void HurtAgent(Agent target)
        {
            if (source) { target.health.Hurt(new HitEvent(ability)); }
            //source destroyed, use pre-calculated damage
            else { target.health.Hurt(new HitEvent() { baseDamage = baseDamage }); }
        }

        //======== Movement ===========
        protected virtual void UpdateMoveDir()
        {

        }

        //============ Lifetime =============
        private IEnumerator LifeTimeCo()
        {
            yield return new WaitForSeconds(lifeTime);
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        //========== Debug =============
        private void OnDrawGizmos()
        {
            //Gizmos.DrawLine(transform.localPosition - new Vector3(0, 0, transform.localScale.z / 2), transform.position + velocity);
        }
    }
}
