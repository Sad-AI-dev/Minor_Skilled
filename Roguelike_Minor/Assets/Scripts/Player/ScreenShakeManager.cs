using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Game.Player
{
    public class ScreenShakeManager : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private CinemachineFreeLook cam;

        [SerializeField] private float maxDistance;
        private List<CinemachineBasicMultiChannelPerlin> shakeComponents;
        public static ScreenShakeManager instance = null;

        private void Awake()
        {
            if(instance != null && instance != this)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }

            shakeComponents = new List<CinemachineBasicMultiChannelPerlin>();
            for(int i = 0; i < 3; i++)
            {
                shakeComponents.Add(cam.GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>());
                cam.GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
            }
        }

        public void ShakeCamera(float intensity, float duration, Vector3 pos)
        {
            StopAllCoroutines();

            float distance = (pos - player.transform.position).magnitude;
            if(distance < 1) distance = 1;

            Debug.Log(distance);

            if(distance < maxDistance)
                StartCoroutine(ShakeCo(intensity, duration, distance));
        }

        public void StopShake()
        {
            StopAllCoroutines();
        }

        private IEnumerator ShakeCo(float intensity, float duration, float distance)
        {
            for (int i = 0; i < 3; i++)
            {
                cam.GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity * 1 / distance;
            }

            float timeElapsed = 0;

            while(timeElapsed < duration)
            {
                yield return null;
                timeElapsed += Time.deltaTime;
                foreach (var shake in shakeComponents)
                {
                    shake.m_AmplitudeGain = intensity * (1 - timeElapsed/duration);
                }
                
            }    
        }
    }
}
