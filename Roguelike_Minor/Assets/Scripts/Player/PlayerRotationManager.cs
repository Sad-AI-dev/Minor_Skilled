using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PlayerRotationManager : MonoBehaviour
    {
        [SerializeField] private GameObject visuals;
        [SerializeField] private float smoothTime = 0.1f;

        private float smoothVelocity;
       

        public void RotatePlayer(float angle)
        {
            StopAllCoroutines();
            StartCoroutine(RotatePlayerCo(angle));
        }

        private IEnumerator RotatePlayerCo(float angle)
        {
            while (Mathf.Abs(visuals.transform.eulerAngles.y - angle) > 0.1f)
            {
                float rotation = Mathf.SmoothDampAngle(visuals.transform.eulerAngles.y, angle, ref smoothVelocity, smoothTime);
                visuals.transform.rotation = Quaternion.Euler(0, rotation, 0);
                yield return null;
            }

            visuals.transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }
}
