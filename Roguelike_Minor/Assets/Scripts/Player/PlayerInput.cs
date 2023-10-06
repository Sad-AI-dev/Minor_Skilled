using Game.Core;
using Game.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Agent agent;
        [SerializeField] private Interactor interactor;

        bool sprinting = false;

        void Update()
        {
            WalkInput();
            SprintInput();
            JumpInput();
            AbilitiesInput();
            InteractInput();
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

        private void AbilitiesInput()
        {
            if(Input.GetMouseButton(0))
            {
                agent.abilities.primary.TryUse();
            }
            if(Input.GetMouseButton(1))
            {
                agent.abilities.special.TryUse();
            }
        }

        private void InteractInput()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                interactor.TryInteract();
            }
        }
    }
}
