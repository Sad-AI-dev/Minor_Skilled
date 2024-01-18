using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.Data;
using System;

namespace Game.Enemy {
    public class Boss_MeleeGrunt_PrimaryBehaviour : MonoBehaviour
    {
        public Ability source;
        public Rigidbody rb;
        public float speed = 8;
        public float increaseToSize;
        float t;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(DestroyAfterSeconds());
        }

        // Update is called once per frame
        void Update()
        {
            t += Time.deltaTime;
            rb.MovePosition(transform.position + (transform.forward * speed * Time.deltaTime));
            transform.localScale = new Vector3(Mathf.Lerp(2.5f, increaseToSize, t/2f), 1, 1);
        }

        IEnumerator DestroyAfterSeconds()
        {
            yield return new WaitForSeconds(2);
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Agent>().health.Hurt(new HitEvent(source));
            }
        }
    }
}
