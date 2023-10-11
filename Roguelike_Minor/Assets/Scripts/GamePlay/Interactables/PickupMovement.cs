using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class PickupMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float amplitude = 1f;
        [SerializeField] private float moveHeight = 0.5f;

        [Header("Rotation Settings")]
        [SerializeField] private float rotateSpeed = 10f;

        //vars
        private float startHeight;

        private void Start()
        {
            startHeight = transform.position.y;
        }

        private void Update()
        {
            Move();
            Rotate();
        }

        private void Move()
        {
            float newHeight = startHeight + Mathf.Sin(Time.time * amplitude) * moveHeight;
            transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
        }

        private void Rotate()
        {
            transform.Rotate(Vector3.up * (rotateSpeed * Time.deltaTime));
        }
    }
}
