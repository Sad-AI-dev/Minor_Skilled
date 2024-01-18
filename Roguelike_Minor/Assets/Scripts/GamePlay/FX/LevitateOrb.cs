using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LevitateOrb : MonoBehaviour
    {
        [Header("Rotation Behaviour")]
        [SerializeField] private float minSpeed = 1f;
        [SerializeField] private float friction = 10f;

        //vars
        private Vector3 rotateDir;
        private float rotateSpeed;

        private void Start()
        {
            rotateDir = GetRandomVector3();
            rotateSpeed = minSpeed;
        }

        private void Update()
        {
            Rotate();
            HandleRotateSpeed();
        }

        //==== Rotate ====
        private void Rotate()
        {
            transform.Rotate(rotateDir * (rotateSpeed * Time.deltaTime));
        }

        //==== Rotate Speed ====
        private void HandleRotateSpeed()
        {
            rotateSpeed = ApplyFriction(rotateSpeed, minSpeed);
        }

        private float ApplyFriction(float value, float minValue)
        {
            if (value > minValue)
            {
                value -= friction * Time.deltaTime;
                //clamp
                if (value < minValue) { value = minValue; }
            }
            return value;
        }

        //======= Add Force ========
        public void AddRandomForce(float force)
        {
            rotateDir = GetRandomVector3();
            rotateSpeed = force;
        }

        //========== Util =============
        private Vector3 GetRandomVector3()
        {
            Vector3 rand = new Vector3(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f)
            );
            return rand.normalized;
        }
    }
}
