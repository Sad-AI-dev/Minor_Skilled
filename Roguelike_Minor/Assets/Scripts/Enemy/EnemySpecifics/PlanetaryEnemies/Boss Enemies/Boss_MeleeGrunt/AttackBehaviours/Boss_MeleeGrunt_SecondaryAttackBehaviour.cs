using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Enemy {
    public class Boss_MeleeGrunt_SecondaryAttackBehaviour : MonoBehaviour
    {
        public Ability source;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Agent>().health.Hurt(new HitEvent(source));
            }
        }
    }
}
