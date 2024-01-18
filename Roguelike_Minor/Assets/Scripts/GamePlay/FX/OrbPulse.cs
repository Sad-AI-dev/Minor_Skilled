using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class OrbPulse : MonoBehaviour
    {
        [Header("Scale Settings")]
        [SerializeField] private float minScale = 0.5f;
        [SerializeField] private float maxScale = 1.5f;
        [SerializeField] private float basePulseSpeed = 1f;

        [HideInInspector] public float pulseSpeed;

        private void Update()
        {
            UpdateScale();
        }

        //======== Pulse Scale ========
        private void UpdateScale()
        {
            transform.localScale = Vector3.one * CalcCurrentScale();
        }
        private float CalcCurrentScale()
        {
            return Mathf.Abs(Mathf.Sin(Time.time * (pulseSpeed + basePulseSpeed))) * (maxScale - minScale) + minScale;
        }
    }
}
