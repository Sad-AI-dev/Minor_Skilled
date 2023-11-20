using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class GroundedChecker : MonoBehaviour
    {
        [SerializeField] private float coyoteTime;

        private Coroutine coyoteCoroutine;
        private PlayerController controller;
        private CharacterController cc;
        private FrictionManager frictionManager;

        public void CheckGrounded()
        {
            if (controller == null)
            {
                controller = GetComponent<PlayerController>();
                cc = GetComponent<CharacterController>();
                frictionManager = GetComponent<FrictionManager>();
            }

            if (!controller.grounded && cc.isGrounded)
                OnTouchGround();
            if (controller.grounded && !cc.isGrounded)
                OnLeaveGround();
        }

        private void OnTouchGround()
        {
            if (coyoteCoroutine != null)
                StopCoroutine(coyoteCoroutine);

            controller.yVelocity = -0.1f;
            controller.activeGravity = 0;
            controller.grounded = true;
            controller.agent.isGrounded = true;
            controller.jumping = false;
            
            frictionManager.SetFriction(frictionTypes.ground);
        }

        private void OnLeaveGround()
        {
            coyoteCoroutine = StartCoroutine(CoyoteTimeCo());
        }

        IEnumerator CoyoteTimeCo()
        {
            float timePassed = 0;

            while (timePassed < coyoteTime)
            {
                timePassed += Time.deltaTime;
                yield return null;
            }

            if(!controller.jumping)
                frictionManager.SetFriction(frictionTypes.air);

            controller.agent.isGrounded = false;
            controller.grounded = false;
        }
    }
}
