using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class OrbPulse : MonoBehaviour
    {
        [Header("Scale Settings")]
        [SerializeField] private float minScale = 0.5f;
        [SerializeField] private float maxScale = 1.5f;
        [SerializeField] private AnimationCurve scaleCurve;

        [Header("Speed Settings")]
        [SerializeField] private float baseSpeed = 1f;
        [SerializeField] private float maxSpeed = 50f;
        [SerializeField] private AnimationCurve speedCurve;

        [HideInInspector] public float progress;

        private float speed;

        private void Update()
        {
            speed = CalcCurrentSpeed();
            UpdateScale();
        }

        //======== Pulse Scale ========
        private void UpdateScale()
        {
            transform.localScale = Vector3.one * CalcCurrentScale();
        }
        private float CalcCurrentScale()
        {
            float timeVar = (Time.time * speed) % 1f;
            return minScale + (scaleCurve.Evaluate(timeVar) * (maxScale - minScale));
        }

        private float CalcCurrentSpeed()
        {
            return baseSpeed + (scaleCurve.Evaluate(progress / 100f) * (maxSpeed - baseSpeed));
        }
    }
}
