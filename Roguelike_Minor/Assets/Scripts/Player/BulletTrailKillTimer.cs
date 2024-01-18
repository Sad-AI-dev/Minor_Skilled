using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class BulletTrailKillTimer : MonoBehaviour
    {
        [SerializeField] private float velocity;

        private float distance;
        private float killTime;

        private Vector3 direction;

        public void Update()
        {
            if(direction != null)
            {
                transform.position += direction * velocity * Time.deltaTime;
            }
        }

        public void SetKillTimer(Vector3 destination)
        {
            direction = (destination - transform.position);
            distance = direction.magnitude;
            direction.Normalize();
            killTime = (distance / velocity);

            StartCoroutine(KillTimerCo());
        }

        private IEnumerator KillTimerCo()
        {
            yield return new WaitForSeconds(killTime);
            //Debug.Log("Object destroyed: " + killTime);
            Destroy(gameObject);

        }
    }
}
