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
        [SerializeField] private GameObject inventory;

        bool sprinting = false;

        private void Start()
        {
            //hide inventory by default
            inventory.SetActive(false);
        }

        void Update()
        {
            WalkInput();
            SprintInput();
            JumpInput();
            AbilitiesInput();
            InteractInput();
            InventoryInput();
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
            if(Input.GetKeyDown(KeyCode.Q))
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

        private void InventoryInput()
        {
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                inventory.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
            }
            if (Input.GetKeyUp(KeyCode.Tab))
            {
                inventory.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
