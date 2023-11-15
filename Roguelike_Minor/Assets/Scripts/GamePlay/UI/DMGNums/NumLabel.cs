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

        [Header("Movement settings")]
        [SerializeField] private float targetHorOffset = 10;
        [SerializeField] private float horMoveSpeed = 1f;
        [Header("Vertical movement settings")]
        [SerializeField] private float arcHeight;
        [SerializeField] private float arcGravity;

        private Vector3 UIOffset;
        //hor speed vars
        private float dirMult;
        //ver speed vars
        private float verSpeed;

        //visible vars
        private bool isVisible;

        private void OnEnable()
        {
            verSpeed = arcHeight;
            UIOffset = Vector3.zero;
            dirMult = Random.Range(0f, 1f) < 0.5f ? -1 : 1;
            //hide by default
            Hide();
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
            float blend = Mathf.Pow(0.5f, horMoveSpeed * Time.deltaTime);
            UIOffset.x = dirMult * Mathf.Lerp(targetHorOffset, Mathf.Abs(UIOffset.x), blend);
        }

        private void UpdateVerSpeed()
        {
            UIOffset.y += verSpeed * Time.deltaTime * 100;
            verSpeed -= arcGravity * Time.deltaTime * 100;
        }

        //============ Update position ==============
        private void TrackPosition()
        {
            if (PointIsInFrostum(trackPos))
            {
                if (!label.enabled) { Show(); }
                label.rectTransform.position = cam.WorldToScreenPoint(trackPos) + UIOffset;
            }
            else if (label.enabled) { Hide(); }
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

        //============== Hide / Show ================
        private void Hide()
        {
            label.enabled = false;
        }

        private void Show()
        {
            label.enabled = true;
        }
    }
}
