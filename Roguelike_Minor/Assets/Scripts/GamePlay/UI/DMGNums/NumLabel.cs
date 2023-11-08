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

        private Vector2 UIOffset;

        private void Update()
        {
            TrackPosition();
        }

        //============ Update position ==============
        private void TrackPosition()
        {
            if (PointIsInFrostum(trackPos))
            {
                label.rectTransform.position = cam.WorldToScreenPoint(trackPos);
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
