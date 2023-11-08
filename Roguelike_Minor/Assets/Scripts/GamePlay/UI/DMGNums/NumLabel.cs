using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game {
    public class NumLabel : MonoBehaviour
    {
        public TMP_Text label;

        [HideInInspector] public Camera cam;
        [HideInInspector] public Vector3 trackPos;

        [Header("Movement Options")]
        [SerializeField] private float horMoveSpeed;
        [SerializeField] private float horMoveSpeedDropoffMult = 0.8f;
        [SerializeField] private float arcHeight;
        [SerializeField] private float arcGravity;

        private Vector3 UIOffset;
        //hor speed vars
        private float horSpeed;
        private int dirMult;
        //ver speed vars
        private float verSpeed;

        private void OnEnable()
        {
            verSpeed = arcHeight;
            UIOffset = Vector3.zero;
            horSpeed = horMoveSpeed;
            dirMult = Random.Range(0f, 1f) < 0.5f ? -1 : 1;
        }

        private void Update()
        {
            UpdateArcOffset();
            TrackPosition();
        }

        //============== Arc Movement ===============
        private void UpdateArcOffset()
        {
            UpdateHorSpeed();
            UpdateVerSpeed();
        }

        private void UpdateHorSpeed()
        {
            UIOffset.x += horSpeed * dirMult;
            horSpeed *= horMoveSpeedDropoffMult;
        }

        private void UpdateVerSpeed()
        {
            UIOffset.y += verSpeed;
            verSpeed -= arcGravity;
        }

        //============ Update position ==============
        private void TrackPosition()
        {
            if (PointIsInFrostum(trackPos))
            {
                label.rectTransform.position = cam.WorldToScreenPoint(trackPos) + UIOffset;
            }
        }

        private bool PointIsInFrostum(Vector3 worldPos)
        {
            Vector3 viewPortPos = cam.WorldToViewportPoint(worldPos);
            return Is01Range(viewPortPos.x) && Is01Range(viewPortPos.y) && viewPortPos.z > 0;
        }
        private bool Is01Range(float num)
        {
            return num > 0f && num < 1f;
        }
    }
}
