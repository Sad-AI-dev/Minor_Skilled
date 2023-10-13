using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Cinemachine;

namespace Game.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject visuals;
        private Agent agent;

        [Header("walk")]
        private float speed;
        private float walkSpeed;
        private float sprintSpeed;
        [SerializeField] private float deceleration;
        private bool isSlowed = false;

        private Vector3 moveDirection;

        [Header("Jump")]
        [SerializeField] private float JumpForce;
        [SerializeField] private float gravity;
        private float yVelocity;
        private float activeGravity;
        private bool grounded;

        [Header("External Components")]
        [SerializeField] private Camera cam;
        [SerializeField] private CinemachineFreeLook freeLookCam;
        private CharacterController cc;

        private float smoothVelocity;
        private float smoothTime = 0.1f;

        Vector3 warpPosition = Vector3.zero;
        private Coroutine slowCo;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            //cam = Camera.main;
            cc = GetComponent<CharacterController>();
            agent = GetComponent<Agent>();
            walkSpeed = agent.stats.walkSpeed;
            sprintSpeed = agent.stats.sprintSpeed;
        }

        private void FixedUpdate()
        {
            UpdateSpeed();
            
            ApplyGravity();

            //allows to directly set position of player
            Physics.SyncTransforms();

            cc.Move((moveDirection * speed) + new Vector3(0, yVelocity, 0));

            CheckGrounded();
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

        public void ToggleSlow(bool slowed)
        {
            if(slowed)
            {
                isSlowed = true;
            }
            else
            {
                isSlowed = false;
            }
        }

        private void UpdateSpeed()
        {
            if(moveDirection.magnitude > 0.1f)
            {
                if (!isSlowed) speed = walkSpeed * sprintSpeed;
                if (isSlowed) speed = walkSpeed;
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
                //Debug.Log("activeGravity: " +  gravity);
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

            //Debug.Log("Ground touched");
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

        public void StartSlowCoroutine(float duration)
        {
            if(slowCo != null)
                StopCoroutine(slowCo);
            slowCo = StartCoroutine(SlowPlayerCo(duration));
        }

        private IEnumerator SlowPlayerCo(float duration)
        {
            isSlowed = true;
            yield return new WaitForSeconds(duration);
            isSlowed = false;
        }

        public void LockCamera()
        {
            freeLookCam.m_YAxis.m_MaxSpeed = 0;
            freeLookCam.m_XAxis.m_MaxSpeed = 0;
        }
    }
}
