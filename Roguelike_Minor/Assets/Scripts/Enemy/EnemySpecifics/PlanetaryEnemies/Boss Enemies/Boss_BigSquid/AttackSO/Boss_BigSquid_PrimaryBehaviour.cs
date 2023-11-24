using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Enemy {
    public class Boss_BigSquid_PrimaryBehaviour : MonoBehaviour
    {
        public StatusEffectSO poisonEffect;
        public float poisonDamageMult;
        public float despawnTimer;
        [HideInInspector] public Agent source;

        IEnumerator DespawnTimerCO()
        {
            yield return new WaitForSeconds(despawnTimer);
            Destroy(this.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("Environment"))
            {
                StartCoroutine(DespawnTimerCO());
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform != source.transform)
            {
                if (other.TryGetComponent<Agent>(out Agent agent))
                {
                    DOTEffect.DOTEffectVars effectVars = agent.effectHandler.AddEffect(poisonEffect) as DOTEffect.DOTEffectVars;

                    //Set Values
                    effectVars.dmg = poisonDamageMult * source.stats.baseDamage;
                    effectVars.source = source;
                }
            }
        }
    }
}
