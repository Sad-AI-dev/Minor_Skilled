using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Game.Core;
using Game.Core.GameSystems;

namespace Game.Player
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineFreeLook cam;
        [SerializeField] private SettingsSO settings;

        //vars
        private bool locked;
        //speed vars
        private Vector2 baseSpeed;
        private Vector2 speed;

        private void Start()
        {
            locked = false;
            //initialize speed vars
            baseSpeed = new Vector2(cam.m_XAxis.m_MaxSpeed, cam.m_YAxis.m_MaxSpeed);
            SetCamSpeeds();
            //listen to event
            EventBus<SettingsChanged>.AddListener(SetCamSpeeds);
        }

        private void SetCamSpeeds(SettingsChanged eventData = null)
        {
            speed = baseSpeed * settings.mouseSensitivity;
            //apply to cam
            if (!locked)
            {
                cam.m_XAxis.m_MaxSpeed = speed.x;
                cam.m_YAxis.m_MaxSpeed = speed.y;
            }
        }

        public void LockCamera()
        {
            locked = true;
            cam.m_XAxis.m_MaxSpeed = 0;
            cam.m_YAxis.m_MaxSpeed = 0;
        }

        public void UnlockCamera()
        {
            locked = false;
            cam.m_XAxis.m_MaxSpeed = speed.x;
            cam.m_YAxis.m_MaxSpeed = speed.y;
        }

        public void ResetCamera(Quaternion lookDirection)
        {
            float angle = Quaternion.Angle(Quaternion.identity, lookDirection);
            if (lookDirection.eulerAngles.y > 180) { angle *= -1; }
            cam.m_XAxis.Value = angle;
            cam.m_YAxis.Value = 0.5f;
        }

        //=== Handle Destroy ===
        private void OnDestroy()
        {
            EventBus<SettingsChanged>.RemoveListener(SetCamSpeeds);
        }
    }
}
