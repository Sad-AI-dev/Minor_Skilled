using System.Collections;
using UnityEngine;

namespace Game.Core
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float lifeTime;
        [SerializeField] private LayerMask layermask;

        [HideInInspector] public Vector3 moveDir;
        [HideInInspector] public Ability ability;
        protected Agent source;

        

        private void Start()
        {
            source = ability.agent;

            StartCoroutine(LifeTimeCo());
        }

        private void FixedUpdate()
        {
            UpdateMoveDir();
            transform.position += moveDir;
        }

        private void Update()
        {
            CheckHitObject();
        }

        private void CheckHitObject()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, moveDir.magnitude, layermask))
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
    }
}
