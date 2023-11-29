using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineFreeLook cam;
        private float xSpeed, ySpeed;

        public void LockCamera()
        {
            xSpeed = cam.m_XAxis.m_MaxSpeed;
            ySpeed = cam.m_YAxis.m_MaxSpeed;
            cam.m_XAxis.m_MaxSpeed = 0;
            cam.m_YAxis.m_MaxSpeed = 0;
        }

        public void UnlockCamera()
        {
            cam.m_XAxis.m_MaxSpeed = xSpeed;
            cam.m_YAxis.m_MaxSpeed = ySpeed;
        }

       
    }
}
