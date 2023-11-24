using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Enemy
{
    public class PGPrimaryBehaviour : MonoBehaviour
    {
        public Ability source;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                other.GetComponent<Agent>().health.Hurt(new HitEvent(source));
                gameObject.SetActive(false);
            }
        }
    }
}
