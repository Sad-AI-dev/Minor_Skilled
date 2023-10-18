using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Game.Core
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float lifeTime;
        [SerializeField] private LayerMask layermask;

        [HideInInspector] public Vector3 velocity;
        [HideInInspector] public Ability ability;
        protected Agent source;

        private void Start()
        {
            source = ability.agent;

            StartCoroutine(LifeTimeCo());

            //Time.timeScale = 0;
        }

        private void FixedUpdate()
        {
            CheckHitObject();
            UpdateMoveDir();
            transform.position += velocity;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Period))
            {
                Time.timeScale += 0.001f;
            }
            if(Input.GetKeyDown(KeyCode.Comma))
            {
                Time.timeScale -= 0.01f;
            }
        }

        private void CheckHitObject()
        {
            Vector3 transformPos = transform.position - new Vector3(0, 0, transform.localScale.z / 2);
            RaycastHit hit;
            if (Physics.Raycast(transformPos, transform.forward, out hit, velocity.magnitude, layermask, QueryTriggerInteraction.Ignore))
            {
                if (!hit.transform.CompareTag(source.tag))
                {
                    
                    OnCollide(hit);
                    gameObject.SetActive(false);
                }
            }
        }

        protected virtual void OnCollide(RaycastHit hit)
        {

        }

        protected virtual void UpdateMoveDir()
        {

        }

        private IEnumerator LifeTimeCo()
        {
            yield return new WaitForSeconds(lifeTime);
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.localPosition - new Vector3(0, 0, transform.localScale.z / 2), transform.position + velocity);
        }
    }
}
