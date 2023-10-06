using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Game.Player.Soldier
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField] private GameObject sphere;

        public float damage;
        public float areaRadius;

        public int ticks;
        public float tickDelay;

        private bool canTick;

        private void Start()
        {
            sphere.transform.localScale *= areaRadius;
        }

        private void Update()
        {
            if(canTick)
            {
                executeTick();
            }
        }

        private void executeTick()
        {
            canTick = false;

            StartCoroutine(WaitForNextTickCo());
        }

        IEnumerator WaitForNextTickCo()
        {
            yield return new WaitForSeconds(tickDelay);
            canTick = true;
        }
    }
}
