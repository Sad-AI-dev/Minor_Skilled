using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.GameSystems
{
    public class SubEmitterSystem : MonoBehaviour
    {
        [SerializeField] private ParticleSystem parentSystem;
        [SerializeField] private List<SubEmitter> subEmitters;

        private bool isPlaying = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                if(!isPlaying)
                {
                    StartCoroutine(PlayCo());
                    isPlaying = true;
                }
                else
                {
                    StopAllCoroutines();
                    isPlaying = false;
                }
            }
        }

        public void Play()
        {
            parentSystem.Play();

            foreach (var subEmitter in subEmitters)
            {
                float chance = Random.Range(0f, 1f);

                if (chance < subEmitter.emitChance)
                    subEmitter.particles.Play();
            }
        }

        private IEnumerator PlayCo()
        {
            yield return new WaitForSeconds(0.2f);
            Play();


            if (isPlaying)
                StartCoroutine(PlayCo());
        }

    }
}
