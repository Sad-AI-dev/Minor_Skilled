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
        private float time = 0f;

        private void Update()
        {
            time += Time.deltaTime;
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
            return minScale + (scaleCurve.Evaluate(time % 1f) * (maxScale - minScale));
        }

        private float CalcCurrentSpeed()
        {
            return baseSpeed + (scaleCurve.Evaluate(progress / 100f) * (maxSpeed - baseSpeed));
        }
    }
}
