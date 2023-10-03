using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;
using static Codice.Client.Commands.WkTree.WorkspaceTreeNode;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private GameObject visuals;

        [Header("Walk")]
        [SerializeField] private float walkingSpeed;
        [SerializeField] private float deceleration;

        [Header("Jump")]
        [SerializeField] private Transform groundedRaycastAnchor;
        [SerializeField] private float jumpForce;
        [SerializeField] private float gravity;
        private float yVelocity;
        private float currentGravity;

        private PlayerController cc;
        private Camera cam;
        private Vector3 moveInput;
        private Vector3 moveDirection;

        private float speed;
        private bool isGrounded = true;


        private float smoothVelocity;
        private float smoothTime = 0.1f;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            cc = GetComponent<PlayerController>();
            cam = Camera.main;
        }

        private void FixedUpdate()
        {
/*            if(!cc.isGrounded)
                ApplyGravity();

            cc.Move((moveDirection * speed) + new Vector3(0, yVelocity, 0));*/
        }

        public void SetMoveDirection(Vector2 moveInput)
        {
            this.moveInput = moveInput;
            if(moveInput.magnitude >= 0.1f)
            {
                float dirAngle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
                float smoothAngle = Mathf.SmoothDampAngle(visuals.transform.eulerAngles.y, dirAngle, ref smoothVelocity, smoothTime);
                visuals.transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
                moveDirection = Quaternion.Euler(0, dirAngle, 0) * Vector3.forward;
                moveDirection.Normalize();

                if (speed < walkingSpeed) speed = walkingSpeed;
            }
            else
            {
                if (speed > 0) speed -= deceleration * Time.deltaTime;
                else speed = 0;
            }
        }

        public void Jump()
        {
/*            if (cc.isGrounded)
            {
                Debug.Log("Space pressed");
                //isGrounded = false;
                yVelocity += jumpForce;
            }*/
        }


        private void ApplyGravity()
        {
            currentGravity += gravity;
            yVelocity -= currentGravity * Time.deltaTime;
        }

        private void CheckGrounded()
        {
            isGrounded = !Physics.BoxCast(groundedRaycastAnchor.position, new Vector3(0.5f, 0.1f, 0.5f), Vector3.down);
            //isGrounded = Physics.Raycast(groundedRaycastAnchor.position, Vector3.down, 0.1f);
            
            if (isGrounded)
            {
                yVelocity = 0;
                currentGravity = 0;
            }
        }

        private void OnDrawGizmos()
        {
/*            Gizmos.DrawWireCube(groundedRaycastAnchor.position - new Vector3(0, 0.05f, 0), new Vector3(0.5f, 0.1f, 0.5f));
            Gizmos.color = cc.isGrounded ? Color.red : Color.green;
            Gizmos.DrawWireCube(groundedRaycastAnchor.position, new Vector3(0.5f, 0.1f, 0.5f));*/
        }
    }
}
