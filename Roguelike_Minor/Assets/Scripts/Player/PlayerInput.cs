using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;

        bool sprinting = false;
        
        void Update()
        {
            WalkInput();
            SprintInput();
            JumpInput();
        }

        private void WalkInput()
        {
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            playerController.SetMoveDirection(moveInput);
        }

        private void SprintInput()
        {
            if(Input.GetKeyDown(KeyCode.LeftShift) && !sprinting)
            {
                playerController.Sprint(true);
                sprinting = true;
                return;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) && sprinting)
            {
                playerController.Sprint(false);
                sprinting = false;
            }
        }

        private void JumpInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerController.Jump();
            }
        }
    }
}
