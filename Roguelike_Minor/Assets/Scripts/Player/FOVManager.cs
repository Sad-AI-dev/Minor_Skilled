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
        [SerializeField] private float zoomInSpeed, zoomOutSpeed;
        private float currentFOV;
        private float FOV;
        private float deltaFOV;

        private float defaultSpeed;
        private float maxSpeed;

        private Agent agent;

        public bool lockFOV = false;

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
            }
        }

        public void SetMinFOV()
        {
            currentFOV = cam.GetRig(0).m_Lens.FieldOfView;
            cam.m_Lens.FieldOfView = Mathf.Lerp(currentFOV, minFOV, zoomInSpeed);
        }
    }
}
