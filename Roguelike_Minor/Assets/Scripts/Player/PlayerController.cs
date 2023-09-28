using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Game.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject visuals;

        [Header("walk")]
        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;
        [SerializeField] private float deceleration;
        private float speed;
        private bool isRunning = false;

        private Vector3 moveDirection;
        private Vector3 velocity;

        [Header("Jump")]
        [SerializeField] private float JumpForce;
        [SerializeField] private float gravity;
        private float yVelocity;
        private float activeGravity;
        private bool grounded;

        private Camera cam;
        private CharacterController cc;

        private float smoothVelocity;
        private float smoothTime = 0.1f;

        

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            cam = Camera.main;
            cc = GetComponent<CharacterController>();
        }

        private void FixedUpdate()
        {
            UpdateSpeed();
            if (cc.isGrounded)
                OnTouchGround();
            else
                ApplyGravity();
            
            cc.Move((moveDirection * speed) + new Vector3(0, yVelocity / 100, 0));
        }

        public void SetMoveDirection(Vector2 moveInput)
        {
            if(moveInput.magnitude >= 0.1f)
            {
                float dirAngle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
                float smoothAngle = Mathf.SmoothDampAngle(visuals.transform.eulerAngles.y, dirAngle, ref smoothVelocity, smoothTime);
                visuals.transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
                moveDirection = Quaternion.Euler(0, dirAngle, 0) * Vector3.forward;
                moveDirection.Normalize();
            }
            else
            {
                moveDirection = Vector3.zero;
            }
        }

        private void UpdateSpeed()
        {
            
            if(moveDirection.magnitude > 0.1f)
            {
                if (!isRunning) speed = walkSpeed;
                if (isRunning) speed = runSpeed;
            }
            else
            {
                speed -= deceleration;
            }

            speed /= 100;
        }

        private void ApplyGravity()
        {
            activeGravity += gravity;
            yVelocity -= activeGravity;
        }

        public void Jump()
        {
            if (!cc.isGrounded) return;
            grounded = false;
            yVelocity += JumpForce;
            Debug.Log(yVelocity);
        }

        private void OnTouchGround()
        {
            yVelocity = 0;
            gravity = 0;
            grounded = true;
        }
    }
}
