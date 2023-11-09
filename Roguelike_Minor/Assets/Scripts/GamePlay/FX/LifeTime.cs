using System.Collections;
using UnityEngine;

namespace Game {
    public class LifeTime : MonoBehaviour
    {
        [SerializeField] private bool destroy = true;
        [SerializeField] private float lifeTime = 1f;

        private void OnEnable()
        {
            StartCoroutine(LifeTimeCo());
        }

        private IEnumerator LifeTimeCo()
        {
            yield return new WaitForSeconds(lifeTime);
            //end object
            if (destroy) { Destroy(gameObject); }
            else { gameObject.SetActive(false); }
        }
    }
}
