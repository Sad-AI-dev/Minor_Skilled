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
        [SerializeField] private float sprintSpeed;
        [SerializeField] private float deceleration;
        private float speed;
        private bool isSprinting = false;

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

        Vector3 warpPosition = Vector3.zero;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            cam = Camera.main;
            cc = GetComponent<CharacterController>();
        }

        private void FixedUpdate()
        {
            UpdateSpeed();
            
            ApplyGravity();

            Physics.SyncTransforms();
            cc.Move((moveDirection * speed) + new Vector3(0, yVelocity, 0));

            CheckGrounded();

            Debug.Log("gravity: " + activeGravity);
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

        public void Sprint(bool sprinting)
        {
            if(sprinting)
            {
                isSprinting = true;
                speed = sprintSpeed;
            }
            else
            {
                isSprinting = false;
                speed = walkSpeed;
            }
        }

        private void UpdateSpeed()
        {
            
            if(moveDirection.magnitude > 0.1f)
            {
                if (!isSprinting) speed = walkSpeed;
                if (isSprinting) speed = walkSpeed * sprintSpeed;
            }
            else
            {
                speed -= deceleration;
            }

            speed /= 100;
        }

        public void Jump()
        {
            if (!grounded) return;
            yVelocity += JumpForce / 100;
            //Debug.Log(yVelocity);
        }

        private void ApplyGravity()
        {
            if(!grounded)
            {
                activeGravity += gravity / 100;
                Debug.Log("activeGravity: " +  gravity);
                yVelocity -= activeGravity;
            }
        }


        private void CheckGrounded()
        {
            if (!grounded && cc.isGrounded)
                OnTouchGround();
            if (grounded && !cc.isGrounded)
                OnLeaveGround();
        }

        private void OnTouchGround()
        {
            yVelocity = -0.1f;
            activeGravity = 0;
            grounded = true;

            Debug.Log("Ground touched");
        }

        private void OnLeaveGround()
        {
            grounded = false;
        }

        public void ResetVelocity()
        {
            speed = 0;
            yVelocity = 0;
        }
        
        public void WarpToPosition(Vector3 newPosition)
        {
            warpPosition = newPosition;
        }

        private void LateUpdate()
        {
            if(warpPosition != Vector3.zero)
            {
                transform.position = warpPosition;
                warpPosition = Vector3.zero;
            }
        }
    }
}
