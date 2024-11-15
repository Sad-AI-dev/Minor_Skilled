using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Game.Core;

namespace Game.Player
{
    public class FOVManager : MonoBehaviour
    {
        [SerializeField] CinemachineFreeLook cam;
        [SerializeField] private float minFOV, defaultFOV, maxFOV;
        [SerializeField] private float zoomInSpeed, resetSpeed, zoomOutSpeed;
        private float currentFOV;
        private float FOV;
        private float deltaFOV;

        private float defaultSpeed;
        private float maxSpeed;

        private Agent agent;

        public bool lockFOV = false;

        private bool canUpdateFOV = false;
        private Coroutine delayUpdateFOV;

        private void Start()
        {
            agent = GetComponent<Agent>();
            defaultSpeed = agent.stats.walkSpeed * agent.stats.sprintSpeed;
            defaultSpeed /= 100;
            maxSpeed = defaultSpeed * 3;

            deltaFOV = maxFOV - defaultFOV;
        }

        public void UpdateFOV(float velocity)
        {
            //.Log(lockFOV);
            //Debug.Log(cam.GetRig(0).m_Lens.FieldOfView);

            if (!lockFOV)
            {
                currentFOV = cam.GetRig(0).m_Lens.FieldOfView;

                if (velocity > defaultSpeed)
                    FOV = defaultFOV + (deltaFOV * ((velocity - defaultSpeed) / maxSpeed));
                else
                {
                    FOV = defaultFOV;
                }

                FOV = Mathf.Clamp(FOV, minFOV, maxFOV);

                cam.m_Lens.FieldOfView = Mathf.Lerp(currentFOV, FOV, zoomOutSpeed);

                if(FOV == defaultFOV)
                    canUpdateFOV = false;
            }
        }

        public void SetMinFOV()
        {
            StopAllCoroutines();
            currentFOV = cam.m_Lens.FieldOfView;
            cam.m_Lens.FieldOfView = Mathf.Lerp(currentFOV, minFOV, zoomInSpeed);
        }

        public void ResetFOV()
        {
            StartCoroutine(ResetFOVCo());
        }

        IEnumerator ResetFOVCo()
        {
            while((defaultFOV - currentFOV) > 0.1f)
            {
                cam.m_Lens.FieldOfView = Mathf.Lerp(currentFOV, defaultFOV, resetSpeed);
                currentFOV = cam.m_Lens.FieldOfView;
                yield return null;
            }

            currentFOV = defaultFOV;
            lockFOV = false;
        }
    }
}
